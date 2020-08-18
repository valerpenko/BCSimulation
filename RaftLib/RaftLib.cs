using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimLib;

namespace RaftLib
{
    public class RAFTNetwork : Network
    {
        Node leader;
        public RAFTNetwork(int nodeCount) : base(nodeCount)
        {
            leader = nodes[0];
        }
        public override void SendMessage(int nodeNum, string msg)
        {
            throw new Exception();
        }
        public void SendCommand(string msg)
        {
            leader.SetState(msg);
            leader.SentBegin();
        }
        //public override string[] NetworkStatus()
        //{
        //    string[] result = new string[nodes.Length];
        //    for (int i = 0; i < nodes.Length; i++)
        //        result[i] = nodes[i].State();
        //    return result;
        //}
    }
    //public class LogBasedNode : Node
    //{
    //    public void AddNodeCommand command){}
    //    public string State() { return ""; }
    //}

    public enum RAFTState { Follower, Candidate, Leader }
    public struct KeyValue { char key; int value; }
    public struct LogEntry
    //contains command for state machine and 
    //term when entry was received by leader(first index is 1)
    {
        public int term;
        public KeyValue command;
    }
    public struct AppendEntriesRezult
    {
        public bool success; //true if follower contained entry matching prevLogIndex and prevLogTerm
        public int term;  //currentTerm for leader to update itself
    }
    public struct RequestVoteRezult
    {
        public bool voteGranted; //true means candidate received vote
        public int term;  //currentTerm for candidate to update itself
    }
    //public class RAFTServer
    //{
    //    private RAFTState state;
    //    private int id;
    //    //Persistent state on all servers: (Updated on stable storage before responding to RPCs)
    //    public int currentTerm; // latest term server has seen (initialized to 0 on first boot, increases monotonically)
    //    public int votedFor; //candidateId that received vote in current term (or null if none)
    //    List<LogEntry> log; //log entries; 

    //    //Volatile state on all servers:
    //    int commitIndex; //index of highest log entry known to be committed (initialized to 0, increases monotonically)
    //    int lastApplied; //index of highest log entry applied to state machine (initialized to 0, increases monotonically)

    //    //Volatile state on leaders:(Reinitialized after election)
    //    int[] nextIndex;    //for each server, index of the next log entry
    //                        //to send to that server(initialized to leader
    //                        //last log index + 1)
    //    int[] matchIndex;   //for each server, index of highest log entry
    //                        //known to be replicated on server
    //                        //(initialized to 0, increases monotonically)

    //    //Invoked by leader to replicate log entries(§5.3); 
    //    //also used as heartbeat(§5.2).
    //    public AppendEntriesRezult AppendEntriesRPC
    //        (int term, //leader’s term
    //            int leaderId, //so follower can redirect clients
    //            int prevLogIndex, // index of log entry immediately preceding new ones
    //            int prevLogTerm, //term of prevLogIndex entry
    //            LogEntry[] newLog,  // entries to store (empty for heartbeat; may send more than one for efficiency)
    //            int leaderCommit // leader’s commitIndex
    //        )
    //    {
    //        if (state != RAFTState.Leader)
    //        {
    //            AppendEntriesRezult rezult;
    //            if (term < currentTerm)//(§5.1)
    //            { rezult.success = false; rezult.term = -1; return rezult; }
    //            else
    //                if (log[prevLogIndex].term != prevLogTerm)//(§5.3)
    //            { rezult.success = false; rezult.term = -1; return rezult; }

    //            //3.If an existing entry conflicts with a new one
    //            //(same index but different terms), 
    //            //delete the existing entry and all that follow it(§5.3)
    //            //4.Append any new entries not already in the log
    //            UpdateLog(prevLogIndex, newLog);
    //            //5.
    //            if (leaderCommit > commitIndex)
    //                commitIndex = Math.Min(leaderCommit, log.Count - 1);

    //            rezult.success = true;
    //            rezult.term = currentTerm;
    //            return rezult;
    //        }
    //        else throw new Exception();
    //    }
    //    private void UpdateLog(int prevLogIndex, LogEntry[] newLog)
    //    {
    //        int pos = 0;
    //        while ((prevLogIndex + 1 + pos < log.Count) &&
    //               (newLog[pos].term == log[prevLogIndex + 1 + pos].term))
    //            pos++;
    //        if (prevLogIndex + 1 + pos < log.Count)
    //        {
    //            while (pos < newLog.Length)
    //            {
    //                if (prevLogIndex + 1 + pos < log.Count)
    //                    log[prevLogIndex + 1 + pos] = newLog[pos];
    //                else
    //                    log.Add(newLog[pos]);
    //            }
    //        }
    //    }
    //    //Invoked by candidates to gather votes(§5.2).
    //    public RequestVoteRezult RequestVoteRPC
    //        (int term, //candidate’s term
    //            int candidateId, //candidate requesting vote
    //            int lastLogIndex,  //index of candidate’s last log entry(§5.4)
    //            int lastLogTerm //term of candidate’s last log entry(§5.4)
    //        )
    //    {
    //        if (state == RAFTState.Candidate)
    //        {
    //            if (term < currentTerm)
    //            {
    //                RequestVoteRezult rezult;
    //                rezult.term = -1;  //!!!!!!!1
    //                rezult.voteGranted = false;
    //                return rezult;
    //            }
    //            if (
    //                ((votedFor == -1) || (votedFor == candidateId)) &&
    //                ((lastLogIndex >= log.Count - 1) && (lastLogTerm >= log[log.Count - 1].term))
    //               )
    //            {
    //                RequestVoteRezult rezult;
    //                rezult.term = currentTerm;  //!!!!!!!!!!!!1
    //                rezult.voteGranted = true;
    //                return rezult;
    //            }
    //        }
    //        else throw new Exception();
    //    }

    //}
}
