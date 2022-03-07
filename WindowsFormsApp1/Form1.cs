using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Printing;
using System.Collections;
using System.Threading;
using Gma.System.MouseKeyHook;
using System.Runtime.InteropServices;

namespace WindowsFormsApp1
{
    public partial class Form1: Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetCursorPos(int X, int Y);

        private IKeyboardMouseEvents m_Events;

        bool isSaved = false;
        public Form1()
        {
            InitializeComponent();
            // указываем в каком формате сохранять файлы
            saveFileDialog1.Filter = "Text File(*.txt)|*.txt|My NotePad File(*.mnf)| *.mnf| CSshar source code (*.cs)| *.cs | C++ source code (*.cpp)| *.cpp| Header source code (*.h)| *.h| C cource code (*.c)|*.c";
            // присваиваем имени файла, имя файла, которое задаст пользователь
            this.IsMdiContainer = true;
            // шрифт по умолчанию 
            richTextBox1.Font = new Font("CRUEL", 12, FontStyle.Regular);
            autocompleteMenu1.Items = File.ReadAllLines("cs-reserv-list.dicr");
            fastColoredTextBox1.Font =  new Font("CRUEL", 12, FontStyle.Regular);
            statusBarPanel1.Text = "Untitled";
            statusBarPanel2.Text = "Строк: ";
            statusBarPanel3.Text = "Символов: ";
            statusBarPanel4.Text = "Не сохранено";
            statusBarPanel5.Text = "UTF-16";
           

        }
       
       
        private void saveHowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // был ли результат диалога нажатие кнопки отмена
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return; //закрываем окно
            string filename = saveFileDialog1.FileName;
            //сохраняем в файл содержимое нашего окна (весь текст в нём)

            if (fastColoredTextBox1.Visible == true)
            {
                File.WriteAllText(filename, fastColoredTextBox1.Text);
            }
            else
            {
                File.WriteAllText(filename, richTextBox1.Text);
            }
            // Сообзение об успешном сохранении файла, в конце работы метода
            isSaved = true;
            MessageBox.Show("Файл успешно сохранён!");
            string newFileName;


            try
            {
                newFileName = filename.Substring(filename.LastIndexOf('\\'), filename.Length - filename.LastIndexOf('\\'));
                statusBarPanel1.Text = newFileName;

            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Вышли за пределы имени файла - ошибка статус бара");
            }
            
            statusBarPanel4.Text = "Сохранено";
        }
        public bool isOpen = false;
        //путь к файлу открытому
        public string fdFileName = "Untitled";
        //имя открытого файла 
        public string nameofthefile = "Untitled";

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // был ли результат диалога нажатие кнопки отмена
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return; //закрываем окно
            string filename = openFileDialog1.FileName;
            int filemode = -1;
            // проверка мода нашего файла, который открывается 
            if (filename.IndexOf(".c") != -1 || filename.IndexOf(".cpp") != -1 || filename.IndexOf(".h") != -1 || filename.IndexOf(".cs") != -1)
                filemode = 1;
            
            
            // если мод программный файл, то используем текст с подсветкой синтаксиса 
            if (filemode == 1)
            {
                fastColoredTextBox1.Visible = true;
                fastColoredTextBox1.Text = File.ReadAllText(openFileDialog1.FileName);
            }
            // если нет - то обычный ртб
            else
            {
                // считываем всю информацию с файла в наш текстовый редактор
                richTextBox1.Text = File.ReadAllText(openFileDialog1.FileName);

            }
          //  byteviewer.SetFile(openFileDialog1.FileName); // добавляем байтвьювер !!!!

            MessageBox.Show("Файл успешно открыт");
            string newFileName= "Untitiled";

            
            try
            {
                 newFileName = filename.Substring(filename.LastIndexOf('\\'), filename.Length - filename.LastIndexOf('\\'));
                 statusBarPanel1.Text = newFileName;
                 nameofthefile = newFileName;

            }
            catch(System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Вышли за пределы имени файла - ошибка статус бара");
            }
            isOpen = true;
            fdFileName = filename;
           
        }
        
        private void M_Events_MouseMove(object sender, MouseEventArgs e)
        {
            statusBarPanel6.Text = string.Format("x={0:0000}; y={1:0000}", e.X, e.Y);
        }

       

        private void Subscribe(IKeyboardMouseEvents events)
        {
            m_Events = events;
            m_Events.MouseMove += M_Events_MouseMove;
        }

        private void Unsubscribe()
        {
            if (m_Events == null) return;
            m_Events.MouseMove -= M_Events_MouseMove;
            m_Events.Dispose();
            m_Events = null;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Unsubscribe();
            Subscribe(Hook.GlobalEvents());
        }

   

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //проверка
            if (richTextBox1.TextLength > 0)
            {
                //копирование
                richTextBox1.Copy();
            }
            if (fastColoredTextBox1.Visible == true)
            {
                if (fastColoredTextBox1.TextLength > 0)
                {
                    //копирование
                    fastColoredTextBox1.Copy();
                }
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //проверка
            if (richTextBox1.TextLength > 0)
            {
                //вставка
                richTextBox1.Paste();
            }
            if (fastColoredTextBox1.Visible == true)
            {
                if (fastColoredTextBox1.TextLength > 0)
                {
                    //вставка
                    fastColoredTextBox1.Paste();
                }
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //проверка
            if (richTextBox1.TextLength > 0)
            {
                //вырезать
                richTextBox1.Cut();
            }
            if (fastColoredTextBox1.Visible == true)
            {
                if (fastColoredTextBox1.TextLength > 0)
                {
                    //вырезать
                    fastColoredTextBox1.Cut();
                }
            }
        }

        private void fontSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            richTextBox1.Font = fontDialog1.Font;

            if (fastColoredTextBox1.Visible == true)
            {
               fastColoredTextBox1.Font = fontDialog1.Font;
            }

        }

        private void colorSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            richTextBox1.BackColor = colorDialog1.Color;

            if (fastColoredTextBox1.Visible == true)
            {
                fastColoredTextBox1.BackColor = colorDialog1.Color;
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //проверка
            if (richTextBox1.TextLength > 0)
            {
                //выделить
                richTextBox1.SelectAll();
            }
            if (fastColoredTextBox1.Visible == true)
            {
                if (fastColoredTextBox1.TextLength > 0)
                {
                    fastColoredTextBox1.SelectAll();
                }
            }
        }
        
        private void richTextBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // обеспечивает появление контекстного меню 
                richTextBox1.ContextMenuStrip = contextMenuStrip1;
            }
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //проверка
            if (richTextBox1.TextLength > 0)
            {
                //копирование
                richTextBox1.Copy();
            }
            if (fastColoredTextBox1.Visible == true)
            {
                if (fastColoredTextBox1.TextLength > 0)
                {
                    //копирование
                    fastColoredTextBox1.Copy();
                }
            }
        }

        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //проверка
            if (richTextBox1.TextLength > 0)
            {
                //вставка
                richTextBox1.Paste();
            }
            if (fastColoredTextBox1.Visible == true)
            {
                if (fastColoredTextBox1.TextLength > 0)
                {
                    //копирование
                    fastColoredTextBox1.Paste();
                }
            }
        }

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //проверка
            if (richTextBox1.TextLength > 0)
            {
                //вырезать
                richTextBox1.Cut();
            }
            if (fastColoredTextBox1.Visible == true)
            {
                if (fastColoredTextBox1.TextLength > 0)
                {
                    //вырезать
                    fastColoredTextBox1.Cut();
                }
            }
        }

        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //проверка
            if (richTextBox1.TextLength > 0)
            {
                //выlделить
                richTextBox1.SelectAll();
            }

            if (fastColoredTextBox1.Visible == true)
            {
                if (fastColoredTextBox1.TextLength > 0)
                {
                    fastColoredTextBox1.SelectAll();
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        // ТЁМНАЯ ТЕМА
        private void bunifuiOSSwitch1_OnValueChange(object sender, EventArgs e)
        {
            string text;
            string text2;
            
            richTextBox1.BackColor = Color.White;
            textBox3.BackColor = Color.White;

            fastColoredTextBox1.BackColor = Color.White;
            menuStrip1.BackColor = Color.LightGray;
            bunifuiOSSwitch1.BackColor = Color.LightGray;
           label1.BackColor = Color.LightGray;

            int length = richTextBox1.TextLength;
            int length2 = textBox3.TextLength;

            menuStrip1.ForeColor = Color.Black;

            text = richTextBox1.Text;
            text2 = textBox3.Text;
            richTextBox1.Text = "";
            textBox3.Text = "";

            
            textBox3.AppendText(text2);
            richTextBox1.AppendText(text);



            richTextBox1.SelectionStart = length;
            richTextBox1.SelectionLength = text.Length;

            textBox3.SelectionStart = length2;
            textBox3.SelectionLength = text2.Length;

            richTextBox1.SelectionColor = Color.Black;
            textBox3.ForeColor = Color.Black;

            contextMenuStrip1.BackColor = Color.White;
            contextMenuStrip1.ForeColor = Color.Black;
            toolStrip1.BackColor = Color.White;
            label1.ForeColor = Color.Black;
            textBox1.BackColor = Color.White;
            textBox2.BackColor = Color.White;
            if (bunifuiOSSwitch1.Value == true)
            {

                richTextBox1.BackColor = Color.FromArgb(34, 36, 49);
                textBox3.BackColor = Color.FromArgb(34, 36, 49);
                 fastColoredTextBox1.BackColor = Color.DarkGray;

                if (fastColoredTextBox1.Visible == true)
                {
                    menuStrip1.BackColor = Color.DarkGray;
                    contextMenuStrip1.BackColor = Color.DarkGray;
                    bunifuiOSSwitch1.BackColor = Color.DarkGray;
                    label1.BackColor = Color.DarkGray;
                    label1.BackColor = Color.DarkGray;
                }
                else
                {
                    menuStrip1.BackColor = Color.FromArgb(34, 36, 49);
                    contextMenuStrip1.BackColor = Color.FromArgb(34, 36, 49);
                    menuStrip1.ForeColor = Color.White;
                    bunifuiOSSwitch1.BackColor = Color.FromArgb(34, 36, 49);
                    label1.BackColor = Color.FromArgb(34, 36, 49);
                    label1.ForeColor = Color.White;
                }
               
               
                toolStrip1.BackColor = Color.Gray;
                textBox1.BackColor = Color.Gray;
                textBox2.BackColor = Color.Gray;
                
                
                // 
                length = richTextBox1.TextLength;
                text = richTextBox1.Text;
                richTextBox1.Text = "";

                length2 = textBox3.TextLength;
                text2 = textBox3.Text;
                textBox3.Text = "";

                
                contextMenuStrip1.ForeColor = Color.White;

                richTextBox1.SelectionColor = Color.White;
                richTextBox1.AppendText(text);
                richTextBox1.SelectionStart = length;
                richTextBox1.SelectionLength = text.Length;

                textBox3.ForeColor = Color.White;
                textBox3.AppendText(text2);
                textBox3.SelectionStart = length2;
                textBox3.SelectionLength = text2.Length;
                
            }


        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }



       
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            string text3;
            if (bunifuiOSSwitch1.Value == true)
            {
                richTextBox1.SelectionColor = Color.White;
            }
           
          text3 = richTextBox1.Text;

          string[] lines = text3.Split('\n');
          statusBarPanel2.Text = "Cтрок: " + lines.Length.ToString();
          statusBarPanel3.Text = "Символов: " + text3.Length.ToString(); 

            if (isHex == true)
            {
                textBox3.Text = ToHex(richTextBox1.Text);
            }
           
            
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
      
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Dispose для ивента
            Unsubscribe();

            if (isSaved == false && richTextBox1.Text != "" || fastColoredTextBox1.Visible == true &&  isSaved == false && fastColoredTextBox1.Text != "")
            {
               //сообщение которое предлагает сохранить файл
                if (MessageBox.Show("Хотите сохранить изменения?", "Мой документ",
                   MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    
                    e.Cancel = true;
                   
                }
            }
            else if (isSaved == false && richTextBox1.Text != "" || fastColoredTextBox1.Visible==true && isSaved == false && fastColoredTextBox1.Text != "")
            {
               
                if (MessageBox.Show("Вы хотите сохранить изменения?", "Мой документ",
                   MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                
                    e.Cancel = true;
                   
                    if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                        return; //закрываем окно
                    string filename = saveFileDialog1.FileName;
                    //сохраняем в файл содержимое нашего окна (весь текст в нём)
                    if (fastColoredTextBox1.Visible == true)
                    {
                        File.WriteAllText(filename, fastColoredTextBox1.Text);
                    }
                    else
                    {
                        File.WriteAllText(filename, richTextBox1.Text);
                    }
                    // Сообзение об успешном сохранении файла, в конце работы метода
                    isSaved = true;
                    MessageBox.Show("Файл успешно сохранён!");
                }

            }
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        // АВТОСОХРАНЕНИЕ ДОКУМЕНТА 
        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.S && e.Modifiers == Keys.Control)
            {
                // получаем выбранный файл
                string filename = saveFileDialog1.FileName;
                // сохраняем текст в файл
                if (richTextBox1.Text == ""  ) 
                    MessageBox.Show("Файл пуст");
                else if (isSaved == false )
                {
                    if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                        return; //закрываем окно
                     filename = saveFileDialog1.FileName;
                    //сохраняем в файл содержимое нашего окна (весь текст в нём)
                   
                    
                    
                        File.WriteAllText(filename, richTextBox1.Text);
                    
                    // Сообзение об успешном сохранении файла, в конце работы метода
                    isSaved = true;
                    MessageBox.Show("Файл успешно сохранён!");
                    
                }
                else if (isSaved == true )
                {
                    filename = saveFileDialog1.FileName;
                    // сохраняем текст в файл
                  
                    
                        File.WriteAllText(filename, richTextBox1.Text);
                    
                    MessageBox.Show("Файл сохранен");
                }
            }
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        string globalfilename;
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename;

            if (isOpen == true)
            {
                isSaved = true;
                
            }
            if (isSaved == false)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return; //закрываем окно
                filename = saveFileDialog1.FileName;
                globalfilename = saveFileDialog1.FileName;
                //сохраняем в файл содержимое нашего окна (весь текст в нём)
                // сохраняем текст в файл
                if (fastColoredTextBox1.Visible == true)
                {
                    File.WriteAllText(filename, fastColoredTextBox1.Text);
                }
                else
                {
                    File.WriteAllText(filename, richTextBox1.Text);
                }
                // Сообзение об успешном сохранении файла, в конце работы метода
                isSaved = true;
                MessageBox.Show("Файл успешно сохранён!");

            }
            else if (isSaved == true)
            {
                if (isOpen == true)
                {
                    filename = fdFileName;
                    globalfilename = fdFileName;
                }

                else
                {
                    filename = saveFileDialog1.FileName;
                    globalfilename = saveFileDialog1.FileName;
                }
                // сохраняем текст в файл
                if (fastColoredTextBox1.Visible == true)
                {
                    File.WriteAllText(filename, fastColoredTextBox1.Text);
                }
                else
                {
                    File.WriteAllText(filename, richTextBox1.Text);
                }
                MessageBox.Show("Файл сохранен");
            }

            string newFileName;

            try
            {
                newFileName = globalfilename.Substring(globalfilename.LastIndexOf('\\'), globalfilename.Length - globalfilename.LastIndexOf('\\'));
                statusBarPanel1.Text = newFileName;

            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Вышли за пределы имени файла - ошибка статус бара");
            }
            statusBarPanel4.Text = "Сохранено";
        }
         

        bool utf1 = true;
        bool iso1 = false;
        bool win12521 = false;

        //1252** - смена кодировки на windows1252 - DONE (из UTF & ISO)
        private void windows1252ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fastColoredTextBox1.Visible == true)
            {
                MessageBox.Show("Изменение кодировки недоступно в режиме разработчика!");
            }
            else
            {
                // из UTF
                if (utf1 == true)
                {
                    string utfLine = richTextBox1.Text;

                    Encoding utf = Encoding.UTF8;
                    Encoding win = Encoding.GetEncoding(1251);

                    byte[] utfArr = utf.GetBytes(utfLine);
                    byte[] winArr = Encoding.Convert(win, utf, utfArr);

                    string winLine = win.GetString(winArr);
                    richTextBox1.Text = winLine;
                    win12521 = true;
                    utf1 = false;
                    statusBarPanel5.Text = "Win1252";
                }
                // из ISO
                else if (iso1 == true)
                {
                    string isoLine = richTextBox1.Text;

                    Encoding win = Encoding.GetEncoding(1251);
                    Encoding iso = Encoding.GetEncoding("ISO-8859-1");

                    byte[] isoiArr = iso.GetBytes(isoLine);
                    byte[] winArr = Encoding.Convert(win, iso, isoiArr);

                    string winLine = win.GetString(winArr);
                    richTextBox1.Text = winLine;
                    win12521 = true;
                    iso1 = false;
                    statusBarPanel5.Text = "Win1252";
                }
            }
        }


        // смена кодировки на ISO из UTF, Win1252
        private void ISOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fastColoredTextBox1.Visible == true)
            {
                MessageBox.Show("Изменение кодировки недоступно в режиме разработчика!");
            }
            else
            {
                // из UTF в ISO
                if (utf1 == true)
                {
                    string utfLine = richTextBox1.Text;

                    Encoding utf = Encoding.UTF8;
                    Encoding iso = Encoding.GetEncoding("ISO-8859-1");

                    byte[] utfArr = utf.GetBytes(utfLine);
                    byte[] isoArr = Encoding.Convert(iso, utf, utfArr);

                    string asciiLine = iso.GetString(isoArr);
                    richTextBox1.Text = asciiLine;
                    iso1 = true;
                    utf1 = false;
                    statusBarPanel5.Text = "ISO-8859-1";
                }

                // из Win1252 в ISO
                else if (win12521 == true)
                {
                    string winLine = richTextBox1.Text;

                    Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                    Encoding win = Encoding.GetEncoding(1251);

                    byte[] winArr = win.GetBytes(winLine);

                    byte[] isoArr = Encoding.Convert(iso, win, winArr);


                    string asciiLine = iso.GetString(isoArr);
                    richTextBox1.Text = asciiLine;
                    win12521 = false;
                    iso1 = true;
                    statusBarPanel5.Text = "ISO-8859-1";
                }

            }
        }
        // смена кодировки на UTF-8
        private void uTF8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fastColoredTextBox1.Visible == true)
            {
                MessageBox.Show("Изменение кодировки недоступно в режиме разработчика!");
            }
            else
            {
                // из iso в utf
                if (iso1 == true)
                {
                    string aisoiLine = richTextBox1.Text;

                    Encoding utf = Encoding.UTF8;
                    Encoding iso = Encoding.GetEncoding("ISO-8859-1");

                    byte[] isoiArr = iso.GetBytes(aisoiLine);
                    byte[] utfArr = Encoding.Convert(utf, iso, isoiArr);

                    string utfLine = utf.GetString(utfArr);
                    richTextBox1.Text = utfLine;
                    utf1 = true;
                    iso1 = false;
                    statusBarPanel5.Text = "UTF-8";
                }
                //из win1252 в utf
                else if (win12521 == true)
                {
                    string winLine = richTextBox1.Text;

                    Encoding utf = Encoding.UTF8;
                    Encoding win = Encoding.GetEncoding(1251);

                    byte[] winArr = win.GetBytes(winLine);

                    byte[] utfArr = Encoding.Convert(utf, win, winArr);


                    string utfLine = utf.GetString(utfArr);
                    richTextBox1.Text = utfLine;
                    win12521 = false;
                    utf1 = true;
                    statusBarPanel5.Text = "UTF-8";
                }
            }
            
        }

       // переходим к значкам 
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            
            richTextBox1.Undo();
            fastColoredTextBox1.Undo();
        }

        private void richTextBox1_Click(object sender, EventArgs e)
        { 
          
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
            fastColoredTextBox1.Redo();
        }

        

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            //проверка
            if (richTextBox1.TextLength > 0)
            {
                //вырезать
                richTextBox1.Cut();
            }
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {

            //проверка
            if (richTextBox1.TextLength > 0)
            {
                //копирование
                richTextBox1.Copy();
            }
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
            fastColoredTextBox1.Undo();
        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
            fastColoredTextBox1.Redo();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            // был ли результат диалога нажатие кнопки отмена
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return; //закрываем окно
            string filename = openFileDialog1.FileName;
            int filemode = -1;
            // проверка мода нашего файла, который открывается 
            if (filename.IndexOf(".c") != -1 || filename.IndexOf(".cpp") != -1 || filename.IndexOf(".h") != -1 || filename.IndexOf(".cs") != -1)
                filemode = 1;


            // если мод программный файл, то используем текст с подсветкой синтаксиса 
            if (filemode == 1)
            {
                fastColoredTextBox1.Visible = true;
                fastColoredTextBox1.Text = File.ReadAllText(openFileDialog1.FileName);
            }
            // если нет - то обычный ртб
            else
            {
                // считываем всю информацию с файла в наш текстовый редактор
                richTextBox1.Text = File.ReadAllText(openFileDialog1.FileName);

            }
            //  byteviewer.SetFile(openFileDialog1.FileName); // добавляем байтвьювер !!!!

            MessageBox.Show("Файл успешно открыт");
            string newFileName = "Untitiled";


            try
            {
                newFileName = filename.Substring(filename.LastIndexOf('\\'), filename.Length - filename.LastIndexOf('\\'));
                statusBarPanel1.Text = newFileName;
                nameofthefile = newFileName;

            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Вышли за пределы имени файла - ошибка статус бара");
            }
            isOpen = true;
            fdFileName = filename;

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            // был ли результат диалога нажатие кнопки отмена
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return; //закрываем окно
            string filename = saveFileDialog1.FileName;
            //сохраняем в файл содержимое нашего окна (весь текст в нём)

            if (fastColoredTextBox1.Visible == true)
            {
                File.WriteAllText(filename, fastColoredTextBox1.Text);
            }
            else
            {
                File.WriteAllText(filename, richTextBox1.Text);
            }
            // Сообзение об успешном сохранении файла, в конце работы метода
            isSaved = true;
            MessageBox.Show("Файл успешно сохранён!");
            string newFileName;


            try
            {
                newFileName = filename.Substring(filename.LastIndexOf('\\'), filename.Length - filename.LastIndexOf('\\'));
                statusBarPanel1.Text = newFileName;

            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Вышли за пределы имени файла - ошибка статус бара");
            }

            statusBarPanel4.Text = "Сохранено";
        }

        public void ChangeTextBox1( string str)
        {
            textBox1.Text = str;
        }
        public void ChangeTextBox2 (string str)
        {
            textBox2.Text = str;
        }
        private void makeToolStripMenuItem_Click(object sender, EventArgs e)
        {


        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void Найти_Click(object sender, EventArgs e)
        {
            //int index = richTextBox1.Text.IndexOf(textBox1.Text);  // узнаем первое вхождение слова (которое будем заменять)
            
        }
        
        // ПОИСК И ЗАМЕНА ПО ТЕКСТУ
        private void button1_Click(object sender, EventArgs e)
        {

           

            if (fastColoredTextBox1.Visible == true)
            {
                if (fastColoredTextBox1.Text.Contains(textBox1.Text))
                {
                    int index = fastColoredTextBox1.Text.IndexOf(textBox1.Text);
                    string str1, str2;
                    str1 = fastColoredTextBox1.Text.Substring(0, index);
                    str2 = fastColoredTextBox1.Text.Substring((index + textBox1.TextLength), (fastColoredTextBox1.TextLength - (index + textBox1.TextLength)));
                    string result = str1 + textBox2.Text + str2;
                    fastColoredTextBox1.Clear();
                    fastColoredTextBox1.AppendText(result);
                    // здесь будем заменять слово без выделения 
                }
                else
                    MessageBox.Show("Такого слова в вашем файле нет!");

            }
            else
            {
                if (richTextBox1.Text.Contains(textBox1.Text)) // проверяем есть ли в rtb такое слово которое (мы - пользователь) ввел для замены
                {
                    int index = richTextBox1.Text.IndexOf(textBox1.Text);  // узнаем первое вхождение слова (которое будем заменять)
                    string str1, str2;
                    str1 = richTextBox1.Text.Substring(0, index); // вырезаем с rtb весь текст до слова (которое будем заменять)
                    str2 = richTextBox1.Text.Substring((index + textBox1.TextLength), (richTextBox1.TextLength - (index + textBox1.TextLength))); // вырезаем весь текст после слова (которое будем заменять)
                    string result = str1 + textBox2.Text + str2; // соединяем 1 строку со 2, между ними вставляем уже новое слово
                    richTextBox1.Clear(); // очищаем от текста rtb 
                    richTextBox1.AppendText(result); // вставляем текст уже с новым словом
                    richTextBox1.Select(str1.Length, textBox2.Text.Length); // выделяем наше новое слово

                    richTextBox1.SelectionFont = new Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));

                }
                else
                    MessageBox.Show("Такого слова в вашем документе нет!"); // в противном случае сообщаем о не найденном слове 
            }
        }
        //создание нового документа 
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Form1 newForm = new Form1();
            newForm.Show();
        }
        // Вывод документа на печать 
        private void PrintDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // объект для печати 
            PrintDocument printDocument = new PrintDocument();

            //устанавливаем обработчик события печати 
            if (fastColoredTextBox1.Visible == true)
            {
                printDocument.PrintPage += PrintPageHandlerProg;
            }
            else
            {
                printDocument.PrintPage += PrintPageHandler;
            }
            // диалоговое окно, которое мы увидим перед печатью
            PrintDialog printDialog = new PrintDialog();
            // устанавливаем объект печати для его настройки 
             printDialog.Document = printDocument;

            // показ диалогового окна с предложением печати 
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                // если результат - ОК, то печатаем 
                printDialog.Document.Print();
            }
        }
        // функция отрисовки текста на странице (ивент)
        void PrintPageHandler(object sender , PrintPageEventArgs e)
        {
            e.Graphics.DrawString(richTextBox1.Text, new Font("CRUEL", 14), Brushes.Black, 0, 0); // brush - используется при отрисовки, координаты верхнего левого угла 0 0
        }
        // отрисовка текста для режима разработчика
        void PrintPageHandlerProg(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(fastColoredTextBox1.Text, new Font("CRUEL", 14), Brushes.Black, 0, 0);
        }
        // дублирование кода для кнопки 
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            // объект для печати 
            PrintDocument printDocument = new PrintDocument();

            //устанавливаем обработчик события печати 
            if (fastColoredTextBox1.Visible == true)
            {
                printDocument.PrintPage += PrintPageHandlerProg;
            }
            else
            {
                printDocument.PrintPage += PrintPageHandler;
            }
            // диалоговое окно, которое мы увидим перед печатью
            PrintDialog printDialog = new PrintDialog();
            // устанавливаем объект печати для его настройки 
            printDialog.Document = printDocument;

            // показ диалогового окна с предложением печати 
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                // если результат - ОК, то печатаем 
                printDialog.Document.Print();
            }
        }
        public bool isItIsSaved = false;
        // запуск режима разработчика
            private void createDevModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isSaved == true)
            {
                isSaved = false;
                isItIsSaved = true;
            }
            richTextBox1.Visible = false;

            fastColoredTextBox1.Visible = true;
            statusBarPanel1.Text = "Untitled";
            statusBarPanel2.Text = "Cтрок: ";
            statusBarPanel3.Text = "Символов: ";
            statusBarPanel4.Text = "Не сохранено";  
            statusBarPanel5.Text = "UTF-8";

            textBox3.Visible = false;
        }

        private void turnOffDevModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fastColoredTextBox1.Visible == false)
                MessageBox.Show(" Вы не вошли в режим разработчика!");
            else
            {
                if (isSaved == false && fastColoredTextBox1.Text != "")
                {
                    if (MessageBox.Show("Хотите сохранить изменения?", "Мой документ",
                   MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {

                       // e.Cancel = true;


                        if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                            return; //закрываем окно
                        string filename = saveFileDialog1.FileName;


                        File.WriteAllText(filename, fastColoredTextBox1.Text);


                        // Сообзение об успешном сохранении файла, в конце работы метода
                        isSaved = true;
                        MessageBox.Show("Файл успешно сохранён!");
                    }
                    fastColoredTextBox1.Visible = false;
                    
                    fastColoredTextBox1.Text = "";
                }
                else
                {
                    fastColoredTextBox1.Visible = false;
                    
                    fastColoredTextBox1.Text = "";
                }
            }
            statusBarPanel2.Text = "Cтрок: ";
            statusBarPanel3.Text = "Символов: ";
             statusBarPanel1.Text = nameofthefile;
            richTextBox1.Visible = true;
          
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
       
        private void fastColoredTextBox1_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
           string text = fastColoredTextBox1.Text;

            string[] lines = text.Split('\n');
            statusBarPanel2.Text = "Cтрок: " + lines.Length.ToString();
            statusBarPanel3.Text = "Символов: " + text.Length.ToString();
           
        }

        private void fastColoredTextBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // обеспечивает появление контекстного меню 
                fastColoredTextBox1.ContextMenuStrip = contextMenuStrip1;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        bool isHex = false;
        // hexView 
        private void createHexViewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (fastColoredTextBox1.Visible == false)
            {
                textBox3.Visible = true;
                isHex = true;
                textBox3.Font = new Font("CRUEL", 12, FontStyle.Regular);
                    textBox3.Text = ToHex(richTextBox1.Text);
            }
            else
            {
                MessageBox.Show("В режиме разработчика HexView не доступен!");
            }
            //rbchanged = false;
       
        }
        // перевод в hex 
        public string ToHex(string inp)
        {
            string outp = string.Empty;
            char[] value = inp.ToCharArray();
            foreach (char L in value)
            {
                int V = Convert.ToInt32(L);
                outp += " " + string.Format("{0:X}", V);
            }
            return outp;
        }
        

            // АВТОСОХРАНЕНИЕ ДОКА ДЛЯ РЕЖИМА РАЗРАБОТЧИКА
            private void fastColoredTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Modifiers == Keys.Control)
            {
                // получаем выбранный файл
                string filename = saveFileDialog1.FileName;
                // сохраняем текст в файл
                if (fastColoredTextBox1.Text == "")
                    MessageBox.Show("Файл пуст");
                else if (isSaved == false)
                {
                    if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                        return; //закрываем окно
                    filename = saveFileDialog1.FileName;
                    //сохраняем в файл содержимое нашего окна (весь текст в нём)



                    File.WriteAllText(filename, fastColoredTextBox1.Text);

                    // Сообзение об успешном сохранении файла, в конце работы метода
                    isSaved = true;
                    MessageBox.Show("Файл успешно сохранён!");

                }
                else if (isSaved == true)
                {
                    filename = saveFileDialog1.FileName;
                    // сохраняем текст в файл


                    File.WriteAllText(filename, fastColoredTextBox1.Text);

                    MessageBox.Show("Файл сохранен");
                }
            }
        }

        private void closeHexViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
            textBox3.Visible = false;
           
        }

        private void statusBar1_PanelClick(object sender, StatusBarPanelClickEventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void hexEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
