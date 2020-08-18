using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using SimLib;
using RaftLib;

namespace ThreadingAttempt
{
    //public interface Observer
    //{
    //    void HandleEvent();
    //}
    public partial class Form1 : Form, Observer
    {
        RAFTNetwork net;
        public Form1()
        {
            InitializeComponent();
            const int linkCount = 42;
            net = new RAFTNetwork(linkCount);
            int[,] links =
            //new int[nodeCount, 2] { { 0, 1 }, { 0, 2 }, { 1, 3 }, { 3, 0 }};
            new int[linkCount, 2] 
            { {0,1}, {0,2}, {0,3}, {0,4 }, {0,5}, { 0, 6 },
              {1,0}, {1,2}, {1,3}, {1,4}, {1,5}, { 1,6 },
              { 2, 0}, { 2,1 }, {2,3 }, {2,4}, {2,5}, {2,6},
              {3,0}, {3,1 }, {3,2}, {3,4}, {3,5}, {3,6},
              {4,0}, {4,1}, {4,2}, {4,3}, {4,5}, {4,6},
              {5,0}, {5 ,1}, {5,2}, {5,3}, {5,4}, {5,6 },
              {6, 0 }, {6, 1 }, {6, 2 }, {6, 3 }, {6, 4 }, {6, 5 }
            };
            net.SetConnections(links);

            net.Model.AddObserver(this);

            net.SendCommand("OK");
            //net.SendMessage("Hello");
            //net.Run();
        }

        public void HandleEvent()
        {
            backgroundWorker1.ReportProgress(5, net);
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // Do not access the form's BackgroundWorker reference directly.
            // Instead, use the reference provided by the sender parameter.
            BackgroundWorker bw = sender as BackgroundWorker;

            // Extract the argument.
            SimModel model = (SimModel)e.Argument;

            // Start the time-consuming operation.
            ModellingProcess(bw, model);
            // If the operation was canceled by the user, 
            // set the DoWorkEventArgs.Cancel property to true.
            if (bw.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                // The user canceled the operation.
                MessageBox.Show("Operation was canceled");
            }
            else if (e.Error != null)
            {
                // There was an error during the operation.
                string msg = String.Format("An error occurred: {0}", e.Error.Message);
                MessageBox.Show(msg);
            }
            else
            {
                // The operation completed normally.
                string msg = String.Format("Result = {0}", e.Result);
                MessageBox.Show(msg);
            }
         
        }

        // This method models an operation that may take a long time 
        // to run. It can be cancelled, it can raise an exception,
        // or it can exit normally and return a result. These outcomes
        // are chosen randomly.
        private void ModellingProcess(BackgroundWorker bw, SimModel model)
        {
            model.Run();
        }
        private void startBtn_Click(object sender, EventArgs e)
        {
            this.backgroundWorker1.RunWorkerAsync(net.Model);
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.backgroundWorker1.CancelAsync();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int k = new Random().Next(0, net.NetwokSize);
            net.SendMessage(k, textBox1.Text);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Network n = (Network)(e.UserState);
            if (n.Model.InProgress)
                listBox1.Items.Add(n.Model.CurNodeCommand.NodeCommandResult); 
            
            else
               listBox1.Items.Add("Моделирование завершено");

        }
    }
   
}
