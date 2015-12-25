using System.Windows.Forms;
using JetCode.BizSchema;

namespace JetCode.MapperConfigure.Controls
{
    public partial class GridParentCtrl : UserControl
    {
        private MappingSchema _mapSchema = null;
        private string _objectName = string.Empty;

        public GridParentCtrl()
        {
            InitializeComponent();
        }

        public void BindingData(MappingSchema mapSchema, string objectName)
        {
            this._mapSchema = mapSchema;
            this._objectName = objectName;

            this.bindingSource1.DataSource = null;
            this.bindingSource1.DataSource = this._mapSchema.Objects[this._objectName].Parents;
        }

        public void UpdateUI()
        {
            this.bindingSource1.ResetBindings(false);
        }
    }
}