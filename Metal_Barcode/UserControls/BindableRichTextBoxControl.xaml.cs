using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Metal_Barcode.Controls
{
    /// <summary>
    /// BindableRichTextBoxControl.xaml 的交互逻辑
    /// </summary>
    public partial class BindableRichTextBoxControl : UserControl
    {
        public static readonly DependencyProperty DocumentProperty =
        DependencyProperty.Register("Document", typeof(FlowDocument), typeof(BindableRichTextBoxControl),
        new PropertyMetadata(OnDocumentChanged));

        public FlowDocument Document
        {
            get { return (FlowDocument)GetValue(DocumentProperty); }
            set
            {
                SetValue(DocumentProperty, value);
            }
        }

        private static void OnDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BindableRichTextBoxControl control = (BindableRichTextBoxControl)d;
            FlowDocument document = e.NewValue as FlowDocument;
            if (document == null)
            {
                control.RTB.Document = new FlowDocument(); //Document is not amused by null :)
            }
            else
            {
                if (document.Blocks.Count > 20)
                {
                    document.Blocks.Remove(document.Blocks.FirstBlock);
                }
                control.RTB.Document = document;
            }

            // 将光标显示到最后一行
            TextPointer t = document.ContentEnd;
            Rect r = t.GetCharacterRect(LogicalDirection.Backward);
            control.RTB.ScrollToVerticalOffset(r.Y);

            // 控制光标位置
            control.RTB.CaretPosition = t;

          
        }
        public BindableRichTextBoxControl()
        {
            InitializeComponent();
        }


        public static readonly DependencyProperty AppendProperty =
       DependencyProperty.Register("Append", typeof(string), typeof(BindableRichTextBoxControl),
       new PropertyMetadata(OnAppendChanged));

        public string Append
        {
            get { return (string)GetValue(AppendProperty); }
            set
            {
                SetValue(AppendProperty, value);
            }
        }

        private static void OnAppendChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BindableRichTextBoxControl control = (BindableRichTextBoxControl)d;
            string document = e.NewValue as string;

            Paragraph pr = new Paragraph();
            if (document.StartsWith("错误"))
                pr.Foreground = (Brush)new BrushConverter().ConvertFromString("#ff0000");
            else if (document.StartsWith("提示"))
                pr.Foreground = (Brush)new BrushConverter().ConvertFromString("#7bbfea");
            else
                pr.Foreground = Brushes.DarkGray;
            pr.Inlines.Add(document);
            control.RTB.Document.Blocks.Add(pr);
            // control.RTB.AppendText(document);

            if (control.RTB.Document.Blocks.Count > 100)
            {
                control.RTB.Document.Blocks.Remove(control.RTB.Document.Blocks.FirstBlock);
            }

            //do { control.RTB.Document.Blocks.Remove(control.RTB.Document.Blocks.FirstBlock); }
            //while (control.RTB.Document.Blocks.Count > 100);

            // 将光标显示到最后一行
            TextPointer t = control.RTB.Document.ContentEnd;
            
            Rect r = t.GetCharacterRect(LogicalDirection.Backward);
            control.RTB.ScrollToVerticalOffset(r.Y);

            // 控制光标位置
            control.RTB.CaretPosition = t;
        }
    }


}
