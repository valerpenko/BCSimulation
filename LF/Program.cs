using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LF
{
    enum NodeState { none, leader, follower }

    interface Executable
    {
        NodeCommand NextCommand();
    }
    class NodeCommand{}
    class SimEnvironment
    {
        List<NodeCommand> deterredCommands;
        while (deterredCommands.Count >0)
    }
    class Node
    {
        NodeState state;
        public Node()
        {
            state = NodeState.none;
            
        }
        public NodeState State
        {
            get { return state; }
            set { state = value; }
        }
        public void Run()
        {
            //analyze environment

        }

        public void LStep()
        {
            if (state==NodeState.leader)
            {
                IssueEvent()
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Node nLeader = new Node(); nLeader.State = NodeState.leader;
            Node nFollower = new Node(); nFollower.State = NodeState.follower;
            nLeader.LStep();
        }
    }
}
