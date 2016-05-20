using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Evolution.Dht.Kademlia
{
    class MaintenanceThread : AncestorThread
    {

        public MaintenanceThread(Client client) : base(client)
        {
            base.thread.Name = "MaintenanceThread";
        }

        protected override void Run()
        {
            var toRemovePeers = new List<PeerId>();
            while (Client.Running)
            {
                //Ping all
                lock (Client.buckets)
                {
                    //Discover
                    foreach (PeerInfo bootstrapPeer in Client.bootstrapPeers)
                    {
                        var mex = new Message(bootstrapPeer);
                        mex.CreateBoostrap(Client.Me.Id, Opcode.BoostrapRequest);
                        Client.Send(mex, bootstrapPeer);

                    }
                    Client.bootstrapPeers.Clear();

                    // TODO: usare lo status dei peer invece di rimuoverli in questa maniera dopo tot tempo
                    // TODO: trasformare le tempistiche in costanti 
                    foreach (PeerInfo peer in Client.buckets)
                    {
                        // Il peer e' stato appena aggiunto?
                        if (DateTime.MinValue == peer.LastSeen)
                        {
                            peer.Status = Status.JustCreated;
                        }
                        // Sono passati piu' di tot sec dall'ultima volta che ho avuto contatti con il peer?
                        else if ((DateTime.Now - peer.LastSeen).TotalMilliseconds > Settings.TIMING_PEER_REMOVE)
                        {
                            if (peer.Retries > Settings.PEER_REMOVE_RETRIES)
                            {
                                peer.Status = Status.ToRemove;
                            }
                            else
                            {
                                peer.Retries++;
                                peer.Status = Status.ToUpdate;
                            }
                        }
                        // sembra un po' a caso, potremmo metterci 5min invece di 1 min?
                        // TODO: Entra sempre qui, da rivedere la logica (mettere toremove prima? oppure cambiarla)
                        else if ((DateTime.Now - peer.LastSend).TotalMilliseconds > Settings.TIMING_PEER_UPDATE ||
                                ((DateTime.Now - peer.LastSeen).TotalMilliseconds > Settings.TIMING_PEER_UPDATE &&
                                    (DateTime.Now - peer.LastSend).TotalMilliseconds > 1 * 60 * 1000))
                        {
                            peer.Status = Status.ToUpdate;
                        }
                        else if ((DateTime.Now - peer.CreatedOn).TotalMilliseconds > Settings.TIMING_PEER_ALWAYS_ON)
                        {
                            peer.Status = Status.AlwaysOn;
                        }
                        else
                        {

                        }

                        switch (peer.Status)
                        {
                            case Status.ToRemove:
                                toRemovePeers.Add(peer.Id);
                                Client.RaiseDeletedPeerEvent(peer);
                                break;
                            case Status.JustCreated:
                                peer.CreatedOn = DateTime.Now;
                                peer.LastSeen = DateTime.Now;
                                break;
                            case Status.ToUpdate:
                                var ping = new Message(peer);
                                ping.CreatePing();
                                Client.Send(ping, peer);
                                break;
                            case Status.AlwaysOn:
                                break;
                            default:
                                break;
                        }
                        Client.RaiseUpdatedPeerEvent(peer);
                    }
                }

                foreach (var peerid in toRemovePeers)
                {
                    Client.buckets.Remove(peerid);
                }
                toRemovePeers.Clear();

                lock (Client.storages)
                {
                    foreach (Storage storage in Client.storages.Values)
                    {
                        if (storage.Expiration < DateTime.Now)
                        {
                            Client.storages.Remove(storage.Key);
                            break;
                        }
                    }
                }
                
                lock (Client.publications)
                {
                    foreach (Storage storage in Client.publications.Values)
                    {
                        if ((storage.Expiration - DateTime.Now).TotalMilliseconds < Settings.TIMING_PUBLICATION_EXPIRATION)
                        {
                            Client.BeginStore(storage.Key, storage.Val);
                            break;
                        }
                    }
                }
                lock (Client.threads)
                {
                    for (int i = 0; i < Client.threads.Count; i++)
                    {
                        if (Client.threads[i].GetType() == typeof(FindValueThread))
                        {
                            var t = Client.threads[i];
                            // Termino il thread se e' passato troppo tempo come da settings
                            if (DateTime.Now.Subtract(t.StartTime).Seconds > Settings.FINDVALUE_MAX_TIMESPAN)
                            {
                                t.Abort();
                                Client.threads.Remove(t);
                            }
                        }
                    }
                }
                System.Threading.Thread.Sleep(Settings.TIMING_REFRESH_CLIENTRUN);
            }
        }
    }
}
