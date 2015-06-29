using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Printing;

public class Estado 
{ 
    public static string TckEstado = ""; 
 } 

namespace BarControls

{
    public class OrderItem
    {
        #region Fields

        char[] delimitador = new char[] { '?' };

        #endregion Fields

        #region Constructors

        public OrderItem(char delimit)
        {
            delimitador = new char[] {delimit };
        }

        #endregion Constructors

        #region Methods

        public string GenerateItem(string cantidad, string itemName, string price)
        {
            return itemName + delimitador[0] + cantidad  + delimitador[0] + price;
        }

        public string GetItemCantidad(string orderItem)
        {
            string [] delimitado = orderItem.Split(delimitador);
            return delimitado[0];
        }

        public string GetItemName(string orderItem)
        {
            string [] delimitado = orderItem.Split(delimitador);
            return delimitado[1];
        }

        public string GetItemPrice(string orderItem)
        {
            string[] delimitado = orderItem.Split(delimitador);
            return delimitado[2];
        }

        #endregion Methods
    }


    public class OrderItem1
    {
        #region Fields

        char[] delimitador = new char[] { '?' };

        #endregion Fields

        #region Constructors

        public OrderItem1(char delimit)
        {
            delimitador = new char[] { delimit };
        }

        #endregion Constructors

        #region Methods

        public string GenerateItem1(string c, string itemN1, string p)
        {
            return itemN1 + delimitador[0] + c + delimitador[0] + p;
        }

        public string GetItemC(string orderItem1)
        {
            string[] delimitado = orderItem1.Split(delimitador);
            return delimitado[0];
        }

        public string GetItemN(string orderItem1)
        {
            string[] delimitado = orderItem1.Split(delimitador);
            return delimitado[1];
        }

        public string GetItemP(string orderItem1)
        {
            string[] delimitado = orderItem1.Split(delimitador);
            return delimitado[2];
        }

        #endregion Methods
    }

       
    public class OrderTotal
    {
        #region Fields

        char[] delimitador = new char[] { '?' };

        #endregion Fields

        #region Constructors

        public OrderTotal(char delimit)
        {
            delimitador = new char[] { delimit };
        }

        #endregion Constructors

        #region Methods

        public string GenerateTotal(string totalName, string price)
        {
            return totalName + delimitador[0] + price;
            //return totalName +  price;
        }

        public string GetTotalCantidad(string totalItem)
        {
            string[] delimitado = totalItem.Split(delimitador);
            return delimitado[1];
        }

        public string GetTotalName(string totalItem)
        {
            string[] delimitado = totalItem.Split(delimitador);
            return delimitado[0];
        }

        #endregion Methods
    }

    public class Ticket
    {
        #region Fields

        public int count = 0;
        string fontName = "Courier New";
        int fontSize = 9;
        ArrayList footerLines = new ArrayList();
        Graphics gfx = null;
        private Image headerImage = null;
        ArrayList headerLines = new ArrayList();
        int imageHeight = 0;
        ArrayList items = new ArrayList();
        ArrayList items1 = new ArrayList();
        float leftMargin = 0;
        string line = null;
        int maxChar = 32;
        int maxCharDescription = 20;
        SolidBrush myBrush = new SolidBrush(Color.Black);
        Font printFont = null;
        ArrayList subHeaderLines = new ArrayList();
        float topMargin = 0;
        ArrayList totales = new ArrayList();
      

        #endregion Fields

        #region Constructors

        public Ticket()
        {
        }

        #endregion Constructors

        #region Properties

        public string FontName
        {
            get { return fontName; }
            set { if (value != fontName) fontName = value; }
        }

        public int FontSize
        {
            get { return fontSize; }
            set { if (value != fontSize) fontSize = value; }
        }

        public Image HeaderImage
        {
            get { return headerImage; }
            set { if (headerImage != value) headerImage = value; }
        }

        public int MaxChar
        {
            get { return maxChar; }
            set { if (value != maxChar) maxChar = value; }
        }

        public int MaxCharDescription
        {
            get { return maxCharDescription; }
            set { if (value != maxCharDescription) maxCharDescription = value; }
        }

        #endregion Properties

        #region Methods

        public void AddFooterLine(string line)
        {
            Estado.TckEstado = "AddFooter";
            footerLines.Add(line);
        }

        public void AddFooterLine1(string line)
        {
            Estado.TckEstado = "AddFooter1";
            footerLines.Add(line);
        }

        public void AddHeaderLine(string line)
        {
            Estado.TckEstado = "AddHeader";
            headerLines.Add(line);
        }

        public void AddItem(string cantidad,string item, string price)
        {
            OrderItem newItem = new OrderItem('?');
            Estado.TckEstado = "AddItem";
            items.Add(newItem.GenerateItem(item, cantidad, price));

        }
        
        public void AddItem1(string c, string i, string p)
        {
            OrderItem1 newItem1 = new OrderItem1('?');
            Estado.TckEstado = "AddItem1";
            items1.Add(newItem1.GenerateItem1(i, c, p));
        }


        public void AddSubHeaderLine(string line)
        {
            subHeaderLines.Add(line);
        }

        public void AddTotal(string name, string price)
        {
            OrderTotal newTotal = new OrderTotal('?');
            totales.Add(newTotal.GenerateTotal(name, price));
        }

        public bool PrinterExists(string impresora)
        {
            foreach (String strPrinter in PrinterSettings.InstalledPrinters)
            {
                if (impresora == strPrinter)
                    return true;
            }
            return false;
        }

        public void PrintTicket(string impresora)
        {
            printFont = new Font(fontName, fontSize, FontStyle.Regular);
            PrintDocument pr = new PrintDocument();
            pr.PrinterSettings.PrinterName = impresora;
            pr.PrintPage += new PrintPageEventHandler(pr_PrintPage);
            pr.Print();
        }
    
        private string AlignRightText(int lenght)
        {
            string espacios = "";
            int spaces = maxChar - lenght;
            for (int x = 0; x < spaces; x++)
                espacios += " ";
            return espacios;
        }

        private string DottedLine()
        {
            string dotted = "";
            for (int x = 0; x < maxChar; x++)
                dotted += "=";
            return dotted;
        }

        private void DrawEspacio()
        {
            line = "";

            gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

            count++;
        }

        private void DrawFooter()
        {
            foreach (string footer in footerLines)
            {
                if (footer.Length > maxChar)
                {
                    int currentChar = 0;
                    int footerLenght = footer.Length;

                    while (footerLenght > maxChar)
                    {
                        line = footer;
                        gfx.DrawString(line.Substring(currentChar, maxChar), printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                        count++;
                        currentChar += maxChar;
                        footerLenght -= maxChar;
                    }
                    line = footer;
                    gfx.DrawString(line.Substring(currentChar, line.Length - currentChar), printFont, myBrush, leftMargin, YPosition(), new StringFormat());
                    count++;
                }
                else
                {
                    line = footer;
                    gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                    count++;
                }
            }
            leftMargin = 0;
            DrawEspacio();
            footerLines.Clear();
        }

        private void DrawHeader()
        {
            DrawEspacio();
            foreach(string header in headerLines)
            {
                if (header.Length > maxChar)
                {
                    int currentChar = 0;
                    int headerLenght = header.Length;

                    while (headerLenght > maxChar)
                    {
                        line = header.Substring(currentChar, maxChar);
                        gfx.DrawString(line, printFont, myBrush, leftMargin,YPosition(), new StringFormat());

                        count++;
                        currentChar += maxChar;
                        headerLenght -= maxChar;
                    }
                    line = header;
                    gfx.DrawString(line.Substring(currentChar, line.Length - currentChar), printFont, myBrush, leftMargin,YPosition(), new StringFormat());
                    count++;
                }
                else
                {
                    line = header;
                    gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                    count++;
                }
            }
            line = DottedLine();
            gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());
            count++;
            headerLines.Clear();
        }

        private void DrawImage()
        {
            if (headerImage != null)
            {
                try
                {
                    gfx.DrawImage(headerImage, new Point((int)leftMargin, (int)YPosition()));
                    double height = ((double)headerImage.Height / 58) * 15;
                    imageHeight = (int)Math.Round(height) + 3;
                }
                catch (Exception)
                {
                }
            }
        }

        private void DrawItemsCabecera()
        {
            OrderItem ordIt = new OrderItem('?');

            gfx.DrawString("Producto Cant.   Precio    Total", printFont, myBrush, leftMargin, YPosition(), new StringFormat());
            count++;

            line = DottedLine();
            gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());
            
            //count++;
            DrawEspacio();

            foreach (string item in items)
            {
                line = ordIt.GetItemCantidad(item);
                
                gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                line = ordIt.GetItemPrice(item);
                line = AlignRightText(line.Length) + line;

               gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                string name = ordIt.GetItemName(item);

                leftMargin = 0;
                if (name.Length > maxCharDescription)
                {
                    int currentChar = 0;
                    int itemLenght = name.Length;

                    while (itemLenght > maxCharDescription)
                    {
                        line = ordIt.GetItemName(item);
                        gfx.DrawString("      " + line.Substring(currentChar, maxCharDescription), printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                        count++;
                        currentChar += maxCharDescription;
                        itemLenght -= maxCharDescription;
                    }

                    line = ordIt.GetItemName(item);
                    gfx.DrawString("      " + line.Substring(currentChar, line.Length - currentChar), printFont, myBrush, leftMargin, YPosition(), new StringFormat());
                    count++;
                }
                else
                {
                    gfx.DrawString("      " + ordIt.GetItemName(item), printFont, myBrush, leftMargin, YPosition(), new StringFormat());
                    count++;
                }
            }

            leftMargin = 0;
           // DrawEspacio();
            //line = DottedLine();

            //gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());
            count = 0;
          //  count++;
           // DrawEspacio();
            line = "";
        }

       private void DrawItems1()
        
           {
            OrderItem1 ordIt = new OrderItem1('?');

            gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());
            //count++;
            foreach (string item1 in items1)
            {
                line = ordIt.GetItemC(item1);
              
                gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());
                
                line = ordIt.GetItemP(item1);
                line = AlignRightText(line.Length) + line;

                gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                string name = ordIt.GetItemN(item1);

                leftMargin = 0;
                if (name.Length > maxCharDescription)
                {
                    int currentChar = 0;
                    int itemLenght = name.Length;

                    while (itemLenght > maxCharDescription)
                    {
                        line = ordIt.GetItemN(item1);
                        gfx.DrawString("      " + line.Substring(currentChar, maxCharDescription), printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                        count++;
                        currentChar += maxCharDescription;
                        itemLenght -= maxCharDescription;
                    }

                    line = ordIt.GetItemN(item1);
                    gfx.DrawString("      " + line.Substring(currentChar, line.Length - currentChar), printFont, myBrush, leftMargin, YPosition(), new StringFormat());
                    count++;
                }
                else
                {
                    gfx.DrawString("      " + ordIt.GetItemN(item1), printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                    count++;
                }
            }

            leftMargin = 0;
            //line = DottedLine();

           // gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());
            // DrawEspacio();
            count = 0;
            line = "";
        }
        
        private void DrawSubHeader()
        {
            foreach (string subHeader in subHeaderLines)
            {
                if (subHeader.Length > maxChar)
                {
                    int currentChar = 0;
                    int subHeaderLenght = subHeader.Length;

                    while (subHeaderLenght > maxChar)
                    {
                        line = subHeader;
                        gfx.DrawString(line.Substring(currentChar, maxChar), printFont, myBrush, leftMargin,YPosition(), new StringFormat());

                        count++;
                        currentChar += maxChar;
                        subHeaderLenght -= maxChar;
                    }
                    line = subHeader;
                    gfx.DrawString(line.Substring(currentChar, line.Length - currentChar), printFont, myBrush, leftMargin, YPosition(), new StringFormat());
                    count++;
                }
                else
                {
                    line = subHeader;
                    gfx.DrawString(line, printFont, myBrush, leftMargin,YPosition(), new StringFormat());
                    count++;
                }
            }
            line = DottedLine();
            gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());
            count++;
           //DrawEspacio();
            subHeaderLines.Clear();
        }

        private void DrawTotales()
        {
            OrderTotal ordTot = new OrderTotal('?');
            line = DottedLine();
            gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());
            count++;
            DrawEspacio();
            DrawEspacio();
           
            foreach (string total in totales)
            {
                line =ordTot.GetTotalCantidad(total);
                line = AlignRightText(line.Length) + line;

                gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());
                leftMargin = 0;

                line = ordTot.GetTotalName(total);
                gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());
                count++;
            }
            leftMargin = 0;
            DrawEspacio();
            DrawEspacio();
        }

        private void pr_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            gfx = e.Graphics;
            if (Estado.TckEstado == "AddHeader")
            {
                DrawImage();
                count = 0;
                DrawHeader();
                DrawSubHeader();
            }
            else 
                if (Estado.TckEstado == "AddItem")
            {
                DrawItemsCabecera();
                items.Clear();
            }
            else if (Estado.TckEstado == "AddItem1")
            {
                DrawItems1();
                items1.Clear();
            }
            else if (Estado.TckEstado == "AddFooter")
            {
                count = 0;
                DrawTotales();
                DrawFooter();
            }
            else if (Estado.TckEstado == "AddFooter1")
            {
                    count = 0;
                    DrawFooter();
                   
             }
            //DrawImage();
            //DrawHeader();
            //DrawSubHeader();
            //DrawItems();
            //DrawItems1();
            //DrawTotales();
            //DrawFooter();

            if (headerImage != null)
            {
                HeaderImage.Dispose();
                headerImage.Dispose();
            }
        }

        private float YPosition()
        {
            return topMargin + (count * printFont.GetHeight(gfx) + imageHeight);
        }

        #endregion Methods
    }
    

    public class TicketNotaCredito
    {
        #region Fields

        int count = 0;
        string fontName = "Courier New";
        int fontSize = 9;
        ArrayList footerLines = new ArrayList();
        Graphics gfx = null;
        private Image headerImage = null;
        ArrayList headerLines = new ArrayList();
        int imageHeight = 0;
        ArrayList items = new ArrayList();
        float leftMargin = 0;
        string line = null;
        int maxChar = 32;
        int maxCharDescription = 20;
        SolidBrush myBrush = new SolidBrush(Color.Black);
        Font printFont = null;
        ArrayList subHeaderLines = new ArrayList();
        float topMargin = 0;
        ArrayList totales = new ArrayList();

        #endregion Fields

        #region Constructors

        public TicketNotaCredito()
        {
        }

        #endregion Constructors

        #region Properties

        public string FontName
        {
            get { return fontName; }
            set { if (value != fontName) fontName = value; }
        }

        public int FontSize
        {
            get { return fontSize; }
            set { if (value != fontSize) fontSize = value; }
        }

        public Image HeaderImage
        {
            get { return headerImage; }
            set { if (headerImage != value) headerImage = value; }
        }

        public int MaxChar
        {
            get { return maxChar; }
            set { if (value != maxChar) maxChar = value; }
        }

        public int MaxCharDescription
        {
            get { return maxCharDescription; }
            set { if (value != maxCharDescription) maxCharDescription = value; }
        }

        #endregion Properties

        #region Methods

        public void AddFooterLine(string line)
        {
            footerLines.Add(line);
        }

        public void AddHeaderLine(string line)
        {
            headerLines.Add(line);
        }

        public void AddItem(string cantidad, string item, string price)
        {
            OrderItem newItem = new OrderItem('?');
            items.Add(newItem.GenerateItem(item, cantidad, price));
        }

        public void AddSubHeaderLine(string line)
        {
            subHeaderLines.Add(line);
        }

        public void AddTotal(string name, string price)
        {
            OrderTotal newTotal = new OrderTotal('?');
            totales.Add(newTotal.GenerateTotal(name, price));
        }

        public bool PrinterExists(string impresora)
        {
            foreach (String strPrinter in PrinterSettings.InstalledPrinters)
            {
                if (impresora == strPrinter)
                    return true;
            }
            return false;
        }

        public void PrintTicket(string impresora)
        {
            printFont = new Font(fontName, fontSize, FontStyle.Regular);
            PrintDocument pr = new PrintDocument();
            pr.PrinterSettings.PrinterName = impresora;
            pr.PrintPage += new PrintPageEventHandler(pr_PrintPage);
            pr.Print();
        }

        private string AlignRightText(int lenght)
        {
            string espacios = "";
            int spaces = maxChar - lenght;
            for (int x = 0; x < spaces; x++)
                espacios += " ";
            return espacios;
        }

        private string DottedLine()
        {
            string dotted = "";
            for (int x = 0; x < maxChar; x++)
                dotted += "=";
            return dotted;
        }

        private void DrawEspacio()
        {
            line = "";

            gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

            count++;
        }

        private void DrawFooter()
        {
            foreach (string footer in footerLines)
            {
                if (footer.Length > maxChar)
                {
                    int currentChar = 0;
                    int footerLenght = footer.Length;

                    while (footerLenght > maxChar)
                    {
                        line = footer;
                        gfx.DrawString(line.Substring(currentChar, maxChar), printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                        count++;
                        currentChar += maxChar;
                        footerLenght -= maxChar;
                    }
                    line = footer;
                    gfx.DrawString(line.Substring(currentChar, line.Length - currentChar), printFont, myBrush, leftMargin, YPosition(), new StringFormat());
                    count++;
                }
                else
                {
                    line = footer;
                    gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                    count++;
                }
            }
            leftMargin = 0;
            DrawEspacio();
        }

        private void DrawHeader()
        {
            foreach (string header in headerLines)
            {
                if (header.Length > maxChar)
                {
                    int currentChar = 0;
                    int headerLenght = header.Length;

                    while (headerLenght > maxChar)
                    {
                        line = header.Substring(currentChar, maxChar);
                        gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                        count++;
                        currentChar += maxChar;
                        headerLenght -= maxChar;
                    }
                    line = header;
                    gfx.DrawString(line.Substring(currentChar, line.Length - currentChar), printFont, myBrush, leftMargin, YPosition(), new StringFormat());
                    count++;
                }
                else
                {
                    line = header;
                    gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                    count++;
                }
            }
            line = DottedLine();
            gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());
            count++;
            //DrawEspacio();
        }

        private void DrawImage()
        {
            if (headerImage != null)
            {
                try
                {
                    gfx.DrawImage(headerImage, new Point((int)leftMargin, (int)YPosition()));
                    double height = ((double)headerImage.Height / 58) * 15;
                    imageHeight = (int)Math.Round(height) + 3;
                }
                catch (Exception)
                {
                }
            }
        }

        private void DrawItems()
        {
            OrderItem ordIt = new OrderItem('?');

            gfx.DrawString("Producto Cant.  P.Neto  P.Bruto", printFont, myBrush, leftMargin, YPosition(), new StringFormat());
            count++;

            line = DottedLine();

            gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

            count++;
            DrawEspacio();

            foreach (string item in items)
            {
                line = ordIt.GetItemCantidad(item);

                gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                line = ordIt.GetItemPrice(item);
                line = AlignRightText(line.Length) + line;

                gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                string name = ordIt.GetItemName(item);

                leftMargin = 0;
                if (name.Length > maxCharDescription)
                {
                    int currentChar = 0;
                    int itemLenght = name.Length;

                    while (itemLenght > maxCharDescription)
                    {
                        line = ordIt.GetItemName(item);
                        gfx.DrawString("      " + line.Substring(currentChar, maxCharDescription), printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                        count++;
                        currentChar += maxCharDescription;
                        itemLenght -= maxCharDescription;
                    }

                    line = ordIt.GetItemName(item);
                    gfx.DrawString("      " + line.Substring(currentChar, line.Length - currentChar), printFont, myBrush, leftMargin, YPosition(), new StringFormat());
                    count++;
                }
                else
                {
                    gfx.DrawString("      " + ordIt.GetItemName(item), printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                    count++;
                }
            }

            leftMargin = 0;
            DrawEspacio();
            line = DottedLine();

            gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());

            count++;
            DrawEspacio();
        }

        private void DrawSubHeader()
        {
            foreach (string subHeader in subHeaderLines)
            {
                if (subHeader.Length > maxChar)
                {
                    int currentChar = 0;
                    int subHeaderLenght = subHeader.Length;

                    while (subHeaderLenght > maxChar)
                    {
                        line = subHeader;
                        gfx.DrawString(line.Substring(currentChar, maxChar), printFont, myBrush, leftMargin, YPosition(), new StringFormat());

                        count++;
                        currentChar += maxChar;
                        subHeaderLenght -= maxChar;
                    }
                    line = subHeader;
                    gfx.DrawString(line.Substring(currentChar, line.Length - currentChar), printFont, myBrush, leftMargin, YPosition(), new StringFormat());
                    count++;
                }
                else
                {
                    line = subHeader;
                    gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());
                    count++;
                }
            }
            line = DottedLine();
            gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());
            count++;
            DrawEspacio();
        }

        private void DrawTotales()
        {
            OrderTotal ordTot = new OrderTotal('?');

            foreach (string total in totales)
            {
                line = ordTot.GetTotalCantidad(total);
                line = AlignRightText(line.Length) + line;

                gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());
                leftMargin = 0;

                line = ordTot.GetTotalName(total);
                gfx.DrawString(line, printFont, myBrush, leftMargin, YPosition(), new StringFormat());
                count++;
            }
            leftMargin = 0;
            DrawEspacio();
            DrawEspacio();
        }

        private void pr_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            gfx = e.Graphics;

            if (Estado.TckEstado == "AddHeader")
            {
                DrawImage();
                DrawHeader();
                DrawSubHeader();
            }
            else if (Estado.TckEstado == "AddItem")
            {
                DrawItems();
            }
            else if (Estado.TckEstado == "AddFooter")
            {
                DrawTotales();
                DrawFooter();
            }

            if (headerImage != null)
            {
                HeaderImage.Dispose();
                headerImage.Dispose();
            }
        }

        private float YPosition()
        {
            return topMargin + (count * printFont.GetHeight(gfx) + imageHeight);
        }

        #endregion Methods

    }
}
