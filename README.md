# VideoVortex
##DrawRectangle
 ```
 System.Drawing.Point start;
 bool blnDraw;
 System.Drawing.Rectangle m_draw_rect = new System.Drawing.Rectangle();
 private void Display_Window_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
 {
     if (e.Button == System.Windows.Forms.MouseButtons.Left)
     {
         start = e.Location;
         blnDraw = true;
     }
 }

 private void Display_Window_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
 {
     if (e.Button == System.Windows.Forms.MouseButtons.Left)
     {
         blnDraw = false;
     }
 }

 private void Display_Window_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
 {
     if (blnDraw)
     {
         int width = Math.Abs(e.Location.X - start.X);
         int height = Math.Abs(e.Location.Y - start.Y);
         m_draw_rect = new System.Drawing.Rectangle(start.X, start.Y, width, height);

         Display_Window.Invalidate();
     }
 }

 private void Display_Window_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
 {
     e.Graphics.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Red, 4), m_draw_rect);
 }
 ```