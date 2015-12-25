using System.Drawing;
using System.Windows.Forms;
using JetCode.BizSchema;

namespace JetCode.MapperConfigure.Controls
{
    public partial class FieldContainerCtrl : UserControl
    {
        private ObjectSchema _srcObject = null;
        private ObjectSchema _dstObject = null;

        public FieldContainerCtrl()
        {
            InitializeComponent();
        }

        public FieldContainerCtrl(ObjectSchema srcObject, ObjectSchema dstObject)
        {
            InitializeComponent();

            this._srcObject = srcObject;
            this._dstObject = dstObject;
        }

        public void Init()
        {
            foreach(FieldSchema item in this._srcObject.Fields)
            {
                if (item.IsJoined)
                    continue;

                FieldCtrl ctlField = new FieldCtrl(item, this._dstObject);
                ctlField.Init();
                ctlField.Dock = DockStyle.Top;
                //ctlField.Location = new System.Drawing.Point(0, this.Controls.Count * 32);
                if(this.Controls.Count % 2 == 0)
                {
                    ctlField.BackColor = Color.Beige;
                }
                else
                {
                    ctlField.BackColor = Color.White;
                }
                this.Controls.Add(ctlField);
                ctlField.BringToFront();
            }
        }

        public void Save()
        {
            this.RemoveJoinField();
            foreach(FieldCtrl item in this.Controls)
            {
                item.Save();
            }
        }

        private void RemoveJoinField()
        {
            for(int i = this._dstObject.Fields.Count - 1; i >= 0; i--)
            {
                FieldSchema item = this._dstObject.Fields[i];
                if (item.IsJoined && item.TableName == this._srcObject.Name)
                {
                    this._dstObject.Fields.Remove(item);
                }
            }
        }
    }
}