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

        private void button1_Click(object sender, EventArgs e)
        {
            //Wiimoteの接続
            this.wm.Connect();

            //イベント関数の登録
            this.wm.WiimoteChanged += wm_WiimoteChanged;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.wm.Disconnect();//Wiiリモコンを切断
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

            //X、Y座標の計算
            float x =
                (wm.WiimoteState.BalanceBoardState.CenterOfGravity.X
                + 20.0f) * 10;    //表示位置(X座標)を求める
            float y =
                (wm.WiimoteState.BalanceBoardState.CenterOfGravity.Y
                + 12.0f) * 10;    //表示位置(Y座標)を求める

            //赤色でマーカを描写
            g.FillEllipse(Brushes.Red, x, y, 10, 10);

            g.Dispose();  //グラフィックスを開放
        }
    }
}
