using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Evolution.Dht.Bootstrap;
using Evolution.Dht.Kademlia;
using NLog;
using Mono.Nat;
using Mono.Nat.Pmp;

namespace TestNode
{
    public partial class frmMain : Form
    {
        // Log purpose
        private static Logger logger;
        private List<PeerInfo> peers;
        private Client client;
        
        public frmMain()
        {
            InitializeComponent();
        }

        delegate void LookupResultDelegate(string key, string val);
        delegate void dgvUpdate();

        void OnLookupResult(string key, string val)
        {
            textBoxFindVal.Text = val;
        }

        void OnAddedPeer(PeerInfo peer)
        {
            if (peers.Contains(peer))
            {
                peers.Remove(peer);
                peers.Add(peer);
            }
            else
            {
                peers.Add(peer);
            }
            this.Invoke(new dgvUpdate(ReBindDataSource));
        }

        void OnUpdatedPeer(PeerInfo peer)
        {
            if (peers.Contains(peer))
            {
                peers.Remove(peer);
                peers.Add(peer);
                this.Invoke(new dgvUpdate(ReBindDataSource));
            }
        }

        void OnDeletedPeer(PeerInfo peer)
        {
            if (peers.Contains(peer)) { peers.Remove(peer); }
            this.Invoke(new dgvUpdate(ReBindDataSource));
        }


        void ReBindDataSource() 
        {
            BindingSource source = new BindingSource();
            source.DataSource = peers;
            dgvPeers.DataSource = source;
        }

        void client_Lookup(string key, string val)
        {
            this.Invoke(new LookupResultDelegate(OnLookupResult), new object[]{key,val});
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            logger = LogManager.GetLogger("TestNode");
            logger.Info("TestNode created");

            peers = new List<PeerInfo>();

            client = new Client();
            client.Lookup += new Client.LookupHandler(client_Lookup);
            client.PeerUpdated += new Client.PeerUpdatedHandler(OnUpdatedPeer);
            client.PeerDeleted += new Client.PeerDeletedHandler(OnDeletedPeer);
            client.PeerAdded += new Client.PeerAddedHandler(OnAddedPeer);

            NatUtility.DeviceFound += DeviceFound;
            NatUtility.DeviceLost += DeviceLost;
        }
        
        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (buttonStart.Text == "Stop")
            {
                client.Abort();
                buttonStart.Text = "Start";
                return;
            }
            buttonStart.Text = "Stop";
            buttonStart.Enabled = false;

            client.Me.Name = textBoxId.Text;

            var listeningAddress = GetHostAddresses(txtIPEndPoint.Text);
            client.Me.EndPoint = new IPEndPoint(listeningAddress, Int32.Parse(uxServerPortUdp.Text));

            // TODO: dovrebbe essere asyncrona perche' ci mette un po' ad aprire le porte
            // O forse bloccante?
            if (uxCbUpnp.Checked)
            {
                UseUPNP(Convert.ToInt32(uxServerPortUdp.Text));
            }

            client.Start();

            //MulticastBootstrap multicast = new MulticastBootstrap();
            //multicast.Start();
            //multicast.Send(Dreams.Kademlia.Message.CreatePingRequest(client.Me));

            textBoxNodeHexId.Text = client.Me.Id.ToString();
            uxServerPortUdp.Text = client.Me.EndPoint.Port.ToString();
            logger.Info("Client started");

            client.DownloadNodes();
            client.AnnounceToHive(uxServerPortUdp.Text);

            buttonStart.Enabled = true;
        }

        private IPAddress GetHostAddresses(string txt)
        {
            txt = txt.Replace(" ", ""); 
            txt = txt.Replace("\t", "");

            if (Dns.GetHostAddresses(txt).Length >0 )
                return Dns.GetHostAddresses(txt)[0];
            return null;
        }

        private void UseUPNP(int p)
        {
            //TODO: Lanciare il discovery in un nuovo thread, aspettare un tot, e poi dichiarare fallimento se non viene lanciato DeviceFound()
            NatUtility.StartDiscovery();
        }

        private void DeviceFound(object sender, DeviceEventArgs args)
        {
            try
            {
                INatDevice device = args.Device;

                logger.Fatal("UPNP Enabled Device found");
                logger.Info("Type: {0}", device.GetType().Name);
                logger.Info("External IP: {0}", device.GetExternalIP());
                
                Mapping mapping = new Mapping(Protocol.Udp, Convert.ToInt32(uxServerPortUdp.Text), Convert.ToInt32(uxServerPortUdp.Text));
                device.CreatePortMap(mapping);
                logger.Info("Create Mapping: protocol={0}, public={1}, private={2}", mapping.Protocol, mapping.PublicPort, mapping.PrivatePort);
                try
                {
                    Mapping m = device.GetSpecificMapping(Protocol.Udp, Convert.ToInt32(uxServerPortUdp.Text));
                    logger.Info("Testing port Mapping passed: protocol={0}, public={1}, private={2}", m.Protocol, m.PublicPort, m.PrivatePort);
                    // Se il portfoward funziona interrompiamo il discovery
                    // NOTA: rileviamo solo il primo router della lista
                    NatUtility.StopDiscovery();
                }
                catch
                {
                    logger.Fatal("Could not get specific mapping");
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex.Message);
                logger.Fatal(ex.StackTrace);
            }
        }

        private void DeviceLost(object sender, DeviceEventArgs args)
        {
            INatDevice device = args.Device;

            logger.Fatal("Device Lost");
            logger.Fatal("Type: {0}", device.GetType().Name);
        }

        private void buttonDump_Click(object sender, EventArgs e)
        {
            textBoxDump.Text = client.Dump();
        }

        private void buttonStore_Click(object sender, EventArgs e)
        {
            client.BeginStore(textBoxStoreKey.Text, textBoxStoreVal.Text);
            textBoxStoreKeyHex.Text = PeerId.CalculateId(textBoxStoreKey.Text).ToString();
        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            client.BeginLookup(textBoxFindKey.Text);
            textBoxFindKeyHex.Text = PeerId.CalculateId(textBoxFindKey.Text).ToString();
        }

        private void btnBootstrap_Click(object sender, EventArgs e)
        {
            PeerInfo peer1 = new PeerInfo();
            peer1.Id = PeerId.CalculateId("peer1");
            peer1.EndPoint = new IPEndPoint(GetHostAddresses(txtBootstrapIP.Text), Convert.ToInt32(txtPort.Text));
            client.BootstrapPeers.Add(peer1);
        }

        private void btnPing_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbxPing.Text.Trim()) || string.IsNullOrEmpty(tbxPort.Text.Trim()))
            {
                return;
            }
            var mex = new Evolution.Dht.Kademlia.Message(new IPEndPoint(GetHostAddresses(tbxPing.Text), int.Parse(tbxPort.Text)));
            mex.CreatePing();
            client.Send(mex, client.Me);
        }


    }
}