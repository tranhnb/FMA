using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Utils;
using Activity.Publishers;

namespace Activity
{
    class AcceptInstallationActivity : Activity
    {
        public AcceptInstallationActivity(GuestInformation guestInfo, int mousePositionX, int mousePositionY)
            : base(guestInfo, mousePositionX, mousePositionY)
        {
            
        }

        protected override void InitPoint()
        {
            base.InitPoint();

            Image img = this.CaptureScreen();
            Point startPoint = new Point(400, 120);
            Point endPoint = new Point(500, 520);
            Color c = Color.FromArgb(176, 200, 56);

            Bitmap bitmap = new Bitmap(img);
            Point? point = this.imageUtils.FindAllPixelLocation(bitmap, c, startPoint, endPoint);
            if (point.HasValue) {
                this.MousePositionX = point.Value.X + 5;
                this.MousePositionY = point.Value.Y + 5;
            }
            
            //Clear memory
            img = null;
            bitmap = null;
        }


    }
}

