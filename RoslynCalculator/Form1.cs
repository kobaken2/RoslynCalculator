using Microsoft.CodeAnalysis.CSharp.Scripting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RoslynCalculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 計算式テキストボックスにてEnterが押下された時に、変数と計算式を元に各処理を行います
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxCalculate_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode != Keys.Return)
            {
                return;
            }

            if (this.textBoxValue.Text.StartsWith("clear(") || this.textBoxValue.Text.StartsWith("delete("))
            {
                // this に対するclaer()およびdelete()メソッドのスクリプト実行
                CSharpScript.RunAsync(this.textBoxValue.Text, globals: this).Wait();
                this.labelMessage.Text = String.Empty;
                this.textBoxValue.Text = String.Empty;
            }
            else
            {
                // 単純な数値計算のスクリプト実行
                MainAsync(this.textBoxValue.Text, this.textBoxValue.Text).Wait();
                this.labelMessage.Text = String.Empty;
                this.textBoxValue.Text = String.Empty;
            }
        }

        /// <summary>
        /// 単純な数値計算のスクリプト実行
        /// </summary>
        /// <param name="valueName">変数名</param>
        /// <param name="method">計算式</param>
        /// <returns>非同期操作オブジェクト</returns>
        private async Task MainAsync(String valueName, String method)
        {
            String temp = String.Empty;

            // 今まで計算した計算式をリスト化する
            foreach (ListViewItem item in this.listView.Items)
            {
                temp += $"var {item.Text} = {item.SubItems[1].Text};";
            }

            // 今回の計算式を結合
            temp += $" {method}";

            var result = await CSharpScript.EvaluateAsync(temp);

            this.listView.Items.Add(new ListViewItem(new String[] { valueName, result.ToString(), result.GetType().Name, method }));
        }

        /// <summary>
        /// リストのクリア
        /// </summary>
        public void clear()
        {
            this.listView.Items.Clear();
        }

        /// <summary>
        /// 指定した変数の削除
        /// </summary>
        /// <param name="valueName">削除する変数名</param>
        public void delete(String valueName)
        {
            foreach (ListViewItem item in this.listView.Items)
            {
                if (item.Text == valueName)
                {
                    this.listView.Items.Remove(item);
                    break;
                }
            }
        }

        private void textBoxValue_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode != Keys.Return)
            {
                return;
            }

            if (this.textBoxValue.Text.StartsWith("clear(") || this.textBoxValue.Text.StartsWith("delete("))
            {
                // this に対するclaer()およびdelete()メソッドのスクリプト実行
                CSharpScript.RunAsync(this.textBoxValue.Text, globals: this).Wait();
                this.labelMessage.Text = String.Empty;
                this.textBoxValue.Text = String.Empty;
            }
            else
            {
                // 単純な数値計算のスクリプト実行
                MainAsync(this.textBoxValue.Text, this.textBoxValue.Text).Wait();
                this.labelMessage.Text = String.Empty;
                this.textBoxValue.Text = String.Empty;
            }

        }
    }

}
