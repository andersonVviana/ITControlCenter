using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace HardwareInventoryWF.Dashboard
{
    public class RoundedPanel : Panel
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Define o raio das bordas arredondadas
            int radius = 30;

            // Cria o caminho para desenhar o painel com cantos arredondados
            GraphicsPath path = new GraphicsPath();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90); // canto superior esquerdo
            path.AddArc(new Rectangle(Width - radius, 0, radius, radius), 270, 90); // canto superior direito
            path.AddArc(new Rectangle(Width - radius, Height - radius, radius, radius), 0, 90); // canto inferior direito
            path.AddArc(new Rectangle(0, Height - radius, radius, radius), 90, 90); // canto inferior esquerdo
            path.CloseAllFigures();

            // Aplica a região com bordas arredondadas ao painel
            this.Region = new Region(path);

            // Opcional: Desenhar uma borda
            using (Pen pen = new Pen(Color.Black, 2)) // Define a cor e a espessura da borda
            {
                e.Graphics.DrawPath(pen, path);
            }
        }
    }
}
