namespace Drs.ConnectService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.processInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.UpdateOrderStatusInstaller = new System.ServiceProcess.ServiceInstaller();
            this.SyncServerFilesInstaller = new System.ServiceProcess.ServiceInstaller();
            this.SendEmailToStoreInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // processInstaller
            // 
            this.processInstaller.Password = null;
            this.processInstaller.Username = null;
            // 
            // UpdateOrderStatusInstaller
            // 
            this.UpdateOrderStatusInstaller.Description = "Service to update order status from stores";
            this.UpdateOrderStatusInstaller.DisplayName = "Drs.UpdateOrderStatus Service";
            this.UpdateOrderStatusInstaller.ServiceName = "UpdateOrderStatus";
            this.UpdateOrderStatusInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // SyncServerFilesInstaller
            // 
            this.SyncServerFilesInstaller.Description = "Service to syncronize DBF files of POS System (DATA)";
            this.SyncServerFilesInstaller.DisplayName = "Drs.SyncServerFiles Service";
            this.SyncServerFilesInstaller.ServiceName = "SyncServerFiles";
            this.SyncServerFilesInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            //  
            // SendEmailToStoreInstaller
            // 
            this.SendEmailToStoreInstaller.Description = "Service to send an email when order was sent to store";
            this.SendEmailToStoreInstaller.DisplayName = "Drs.SendEmailToStore Service";
            this.SendEmailToStoreInstaller.ServiceName = "SendEmailToStore";
            this.SendEmailToStoreInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.processInstaller,
            this.UpdateOrderStatusInstaller,
            this.SyncServerFilesInstaller,
            this.SendEmailToStoreInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller processInstaller;
        private System.ServiceProcess.ServiceInstaller UpdateOrderStatusInstaller;
        private System.ServiceProcess.ServiceInstaller SyncServerFilesInstaller;
        private System.ServiceProcess.ServiceInstaller SendEmailToStoreInstaller;
    }
}