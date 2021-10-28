using Microsoft.AspNetCore.Components.Web;
using NoJsBlazor;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ManualTesting {
    public partial class Home : PageComponentBase, IDisposable {
        #region ContextMenu

        [AllowNull]
        private ContextMenu contextMenu;

        private void OpenContextMenu(MouseEventArgs e) {
            contextMenu.Open(e.OffsetX, e.OffsetY);
            Root.MouseDown += CloseContextMenu;
        }

        private void CloseContextMenu(MouseEventArgs e) {
            contextMenu.Close();
            Root.MouseDown -= CloseContextMenu;
        }

        #endregion


        #region Dialog

        private Dialog dialog;

        private void OpenDialog(MouseEventArgs e) => dialog.Open();

        private void CloseDialog(MouseEventArgs e) => dialog.Close();

        #endregion


        public void Dispose() {
            Root.MouseDown -= CloseContextMenu;
        }
    }
}
