
using System;
using System.Collections.Generic;

namespace RaftPrototype1
{
    class Program
    {
        static void Main(string[] args)
        {   RaftEnv re = new RaftEnv();
            Node l = new Node(re);  Node f = new Node(re);
            l.SetPartner(f);  f.SetPartner(l);
            re.AssignLeader(l); re.AddFollower(f);
            l.Step();
            re.Run();
        }
    }
    enum NodeState
    {   None, LeaderBeforeSending, LeaderGotConfirmation, Follower}
    class Node
    {   RaftEnv re;
        NodeState state = NodeState.None;
        Node partner;
        public Node(RaftEnv re) { this.re = re; }
        public NodeState State
        {   get { return state;  }
            set { state = value; }
        }
        public void SetPartner(Node partner) { this.partner = partner; }
        public void Step()
        {   switch (state)
            {   case NodeState.LeaderBeforeSending:
                    re.AddCommand(new LeaderHeartBeatCmd(re.Time + 1.5, this, partner));
                    break;
                case NodeState.LeaderGotConfirmation:
                    break;
                case NodeState.Follower:
                    //when follower gets RPC from Leader and follower has to confirm this
                    re.AddCommand(new ConfirmationFromFollower(re.Time + 0.2, this, partner));
                    break;
            }
        }
    }
    class RaftEnv
    {   Node leader, follower;
        double time=0.0;
        List<TimedCommand> commands = new List<TimedCommand>();
        public double Time
        {   get { return time; }
            set { time = value; }
        }
        public void AssignLeader(Node n)
        {   leader = n;
            leader.State = NodeState.LeaderBeforeSending;
        }
        public void AddFollower(Node n)
        {   follower = n;
            follower.State = NodeState.Follower;
        }
        public void AddCommand(TimedCommand cmd)
        {
            if (commands.Count == 0)
            {   commands.Add(cmd);  }
            else
            {   int pos;
                for(pos=0;pos<commands.Count;pos++)
                {   if (cmd.Time < commands[pos].Time) break;
                }
                if(pos<commands.Count) commands.Insert(pos, cmd); 
                else commands.Add(cmd);
            }
            showCommands();
        }
        private void showCommands()
        {   foreach (TimedCommand cmd in commands)
                Console.WriteLine(cmd);
            Console.ReadLine();
        }
        public void Run()
        {   if (commands.Count == 0)
            {   Console.WriteLine("Execution completed"); }
            else
            {   TimedCommand cmd = commands[0];
                commands.RemoveAt(0);
                cmd.Execute(this);
            }
        }
    }
    abstract class TimedCommand
    {   protected double time;
        protected Node sender;
        public void Execute(RaftEnv re)
        {   re.Time = this.time;
            InnerExecute();
            re.Run();
        }
        public override string ToString()
        {
            return String.Format("type:{0} time:{1}",GetType().Name, time);
        }
        public abstract void InnerExecute();
        public TimedCommand(double time, Node sender)
        {   this.time = time;
            this.sender = sender;
        }
        public double Time
        {   get { return time; }
            set { time = value; }
        }
    }
    class LeaderHeartBeatCmd : TimedCommand
    {   Node follower;  //supposed to be a follower
        public LeaderHeartBeatCmd(double time, Node leader, Node follower)
            : base(time, leader)
        {   this.follower = follower;  }
        public override void InnerExecute()
        {   follower.Step();
            sender.Step();
        }
    }
    class ConfirmationFromFollower : TimedCommand
    {   Node receiver; //supposed to be a leader
        public ConfirmationFromFollower(double time, Node sender,  Node receiver)
            : base(time, sender)
        {   this.receiver = receiver;   }
        public override void InnerExecute()
        {
            receiver.State = NodeState.LeaderGotConfirmation;
            receiver.Step();
        }
    }
        //class LeaderSendCommand : TimedCommand
        //{
        //    Node receiver;
        //    string message;
        //    public LeaderSendCommand(double time, Node receiver, string message)
        //        :base(time)
        //    {
        //        this.receiver = receiver;
        //        this.message = message;
        //    }
        //    public override void Execute(RaftEnv re)
        //    {
        //        base.Execute(re);
        //        receiver.Get(message);
        //    }
        //}
}
