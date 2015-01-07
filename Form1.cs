// Codes by http://www.kosaka-lab.com/tips/2009/02/wiiwii-fit.php

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WiimoteLib;    //WimoteLibの宣言

namespace WiiFitViewer
{
    public partial class Form1 : Form
    {
        Wiimote wm = new Wiimote();

        public Form1()
        {
            InitializeComponent();
            //他スレッドからのコントロール呼び出し許可
            Control.CheckForIllegalCrossThreadCalls = false;  
        }

        void wm_WiimoteChanged(object sender, WiimoteChangedEventArgs args)
        {
            //WiimoteStateの値を取得
            WiimoteState ws = args.WiimoteState;

            //ピクチャーボックスへ描画
            this.DrawForms(ws);

            //ラベル

            //重さ(Kg)表示
            this.label1.Text = ws.BalanceBoardState.WeightKg + "kg";
            //重心のX座標表示
            this.label2.Text = "X:" +
                ws.BalanceBoardState.CenterOfGravity.X;
            //重心のY座標表示
            this.label3.Text = "Y:" +
                ws.BalanceBoardState.CenterOfGravity.Y;
        }

        //フォーム描写関数
        public void DrawForms(WiimoteState ws)
        {
            //pictureBox1のグラフィックスを取得
            Graphics g = this.pictureBox1.CreateGraphics();
            g.Clear(Color.Black);     //画面を黒色にクリア

            float cx = this.pictureBox1.Width / 2f;
            float cy = this.pictureBox1.Height / 2f;
            float xfac = this.pictureBox1.Width / 40f;
            float yfac = this.pictureBox1.Height / 24f;
            //X、Y座標の計算
            float x = cx + 
                (wm.WiimoteState.BalanceBoardState.CenterOfGravity.X) * xfac;    //表示位置(X座標)を求める
            float y = cy + 
                (wm.WiimoteState.BalanceBoardState.CenterOfGravity.Y) * yfac;    //表示位置(Y座標)を求める

            //赤色でマーカを描写
            int rad = 30;
            g.FillEllipse(Brushes.White, cx -rad / 2f, cy -rad / 2f, 30, 30);
            g.FillEllipse(Brushes.Red, x - rad / 2f, y - rad / 2f, 30, 30);

            g.Dispose();  //グラフィックスを開放
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Wiimoteの接続
            this.wm.Connect();
            //イベント関数の登録
            this.wm.WiimoteChanged += wm_WiimoteChanged;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.wm.Disconnect();//Wiiリモコンを切断
        }
    }
}
