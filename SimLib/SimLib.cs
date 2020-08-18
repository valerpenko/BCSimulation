using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SimLib
{
    public class Network
    {
        protected Node[] nodes;
        private SimModel model;
        public Network(int nodeCount)
        {
            nodes = new Node[nodeCount];
            model = new SimModel();
            for (int i = 0; i < nodeCount; i++)
                nodes[i] = new Node(char.ToString(Convert.ToChar('A' + i)), model);
        }
        public SimModel Model{get{return model;} }
        public virtual string[] NetworkStatus()
        {
            string[] result = new string[nodes.Length];
            for (int i= 0; i < nodes.Length; i++)
                result[i]=nodes[i].ToString();
            return result;
        }
        public void Run() { model.Run(); }
        public virtual void SendMessage(int nodeNum, string msg)
        {
            nodes[nodeNum].SetState(msg);
            nodes[nodeNum].SentBegin();
        }
        public void SetConnections(int[,] links)
        {
            for (int i = 0; i < links.GetLength(0); i++)
                nodes[links[i, 0]].ConnectWith(nodes[links[i, 1]]);
        }
        public int NetwokSize { get { return nodes.Length; } }
    }
 

    public class Node
    {
        private string name;
        private string state;
        private SimModel model;
        private List<Node> neighbors;
        public Node(string name, SimModel model)
        {
            this.name = name;
            this.model = model;
            this.state = null;
            neighbors = new List<Node>();
        }
        public String Name { get { return name; } }
        public string GetMessage()
        {
            return state;
        }
        public void SetState(string msg)
        {
            state = msg;
        }
        public override string ToString()
        {
            return String.Format("node {0} state {1}", name, state);
        }
        public void ConnectWith(Node node)
        {
            neighbors.Add(node);
        }
        public void SentBegin()
        {
            //Something usefull
            //this.message = msg;
            double waitTime;
            double mean = 5;
            double deviation;
            foreach (Node n in neighbors)
            {
                deviation = RandomGen.GetInstance().NextDouble();
                waitTime = mean + deviation;
                model.AddCommand(this, n, waitTime);
                //Console.WriteLine("Узел {0} отправляет узлу {1} сообщение {2}", this.name, n.name, this.GetMessage());
            }
        }
    }

    public class NodeCommand
    {
        private Node nodeFrom;
        private Node nodeTo;
        private double time;
        private string nodeCommandResult;
        public NodeCommand(Node nodeFrom, Node nodeTo, double time)
        {
            this.nodeFrom = nodeFrom;
            this.nodeTo = nodeTo;
            this.time = time;
            nodeCommandResult = "";
        }
        public string NodeCommandResult { get { return nodeCommandResult; } }
        public void ActionFinish()
        {
            if (nodeTo.GetMessage() != nodeFrom.GetMessage())
            {
                nodeTo.SetState(nodeFrom.GetMessage());
                nodeTo.SentBegin();
                nodeCommandResult= String.Format("Узел {0} получил от узла {1} сообщение {2}", nodeTo.Name, nodeFrom.Name, nodeFrom.GetMessage());
            }
            else
            {
                nodeCommandResult = String.Format("Узел {0} уже получил сообщение {1}", nodeTo.Name, nodeFrom.GetMessage());
            }
        }
        public double Time { get { return time; } }
    }

    public interface Observer
    {
        void HandleEvent();
    }

    public interface Observable
    {
        void AddObserver(Observer o);
        void NotifyObserver();
    }

    public class SimModel : Observable
    {
        private Observer observer;
        private List<NodeCommand> commandBag;
        private double curTime;
        private bool inProgress = false;
        private NodeCommand curNodeCommand;
        private bool justFinished = false;
        public SimModel()
        {
            commandBag = new List<NodeCommand>();
            curTime = 0.0;
            observer = null;
        }
        public NodeCommand CurNodeCommand { get { return curNodeCommand; } } 
        public void AddObserver(Observer o)
        { observer = o; }
        public void NotifyObserver()
        {
            if (observer!=null)
                observer.HandleEvent();
        }
        public bool InProgress { get { return inProgress; } }
        public void AddCommand(Node nodeFrom, Node nodeTo, double waitTime)
        {
            //при добавлении длительность команды трансформируется в момент ее выполнения
            NodeCommand nc = new NodeCommand(nodeFrom, nodeTo, curTime + waitTime);
            //и добавляется в порядке возрастания в список, т.е. с конца списка
            int listPosition = commandBag.Count; //номер после последнего элемента
            while ((listPosition > 0) && (commandBag[listPosition - 1].Time > nc.Time))
            {
                listPosition--;
            }
            commandBag.Insert(listPosition, nc);
            inProgress = true;
        }
        public void Run()
        {
            while (true)
            {
                Thread.Sleep(500);
                if (commandBag.Count > 0)
                {
                    inProgress = true;
                    justFinished = false;
                    curNodeCommand = SelectNearest();
                    curTime = curNodeCommand.Time;
                    curNodeCommand.ActionFinish();
                    NotifyObserver();
                    //Console.WriteLine("Время модели={0}", curTime);
                    commandBag.Remove(curNodeCommand);
                    if (commandBag.Count == 0) justFinished = true;
                }
                else
                {
                    inProgress = false;
                    if (justFinished)
                    {
                        NotifyObserver();
                        justFinished = false;
                    }
                }
            }
        }
        private NodeCommand SelectNearest()
        {
            return commandBag[0];
        }
    }

    public class RandomGen
    {
        private static Random instance;
        private RandomGen() { }
        public static Random GetInstance()
        {
            if (instance == null) instance = new Random();
            return instance;
        }
    }
}
