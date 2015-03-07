using mshtml;
using System;
using System.IO;
using System.Windows.Forms;

namespace LiveHTML
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnInject_Click(object sender, EventArgs e)
        {
            try
            {
                //Inject a JavaScript <Script></Script> tag in the HEAD of the HTML.
                HtmlElement headElement = webBrowser.Document.GetElementsByTagName("head")[0];
                HtmlElement scriptElement = webBrowser.Document.CreateElement("script");
                IHTMLScriptElement domScriptElement = (IHTMLScriptElement)scriptElement.DomElement;
                domScriptElement.text = "function applyChanges(){" + txtScript.Text + "}";
                headElement.AppendChild(scriptElement);
                Text = "LiveHTML - Successfully Injected";
            }
            catch { Text = "LiveHTML - Injection Failed"; }
        }

        private void btnHTML_Click(object sender, EventArgs e)
        {
            //Refresh the current page's HTML in the richTextBox.
            txtHTML.Text = webBrowser.DocumentText;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            //Invoke the newly injected <Script></Script> tag.
            webBrowser.Document.InvokeScript("applyChanges");
            Text = "LiveHTML - Injected Script Active";
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            //Undo the injected <Script></Script> actions.
            webBrowser.Refresh();
        }

        private void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            //Set the textbox text property to the URL of the current page.
            txtURL.Text = webBrowser.Url.ToString();

            //Print the current page's HTML to the richTextBox.
            txtHTML.Text = webBrowser.DocumentText;

            //Change the window's text to match the new page.
            Text = "LiveHTML - " + webBrowser.DocumentTitle;
        }

        private void txtURL_KeyUp(object sender, KeyEventArgs e)
        {
            //When a new URL is entered into the navbar, navigate to new URL.
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter)
            {
                webBrowser.Navigate(txtURL.Text);
            }
        }

        private void txtFind_KeyUp(object sender, KeyEventArgs e)
        {
            //When a new URL is entered into the navbar, navigate to new URL.
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter)
            {
                if (txtFind.Text.Length > 0)
                {
                    int index = -1;
                    int searchStart = 0;
                    while ((index = txtHTML.Find(txtFind.Text, searchStart, RichTextBoxFinds.None)) > -1)
                    {
                        txtHTML.Select(index, txtFind.Text.Length);
                        txtHTML.SelectionBackColor = System.Drawing.Color.Yellow;
                        searchStart = index + txtFind.Text.Length;
                    }
                }
            }
        }

        private void txtFind_Enter(object sender, EventArgs e)
        {
            if (txtFind.Text == "Find text...") {
                txtFind.Text = "";
                txtFind.Font = new System.Drawing.Font(txtFind.Font, System.Drawing.FontStyle.Regular);
            }
        }

        private void txtFind_Leave(object sender, EventArgs e)
        {
            if (txtFind.Text == "")
            {
                txtFind.Text = "Find text...";
                txtFind.Font = new System.Drawing.Font(txtFind.Font, System.Drawing.FontStyle.Italic);
            }
        }
    }
}
