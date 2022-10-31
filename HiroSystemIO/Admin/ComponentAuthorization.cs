namespace HiroSystemIO.Admin
{
    internal class ComponentAuthorization
    {
        private static ComponentAuthorization account_instance = null;
        public static ComponentAuthorization account_admin
        {
            get
            {
                if (account_instance == null)
                {
                    account_instance = new ComponentAuthorization();
                }
                return account_instance;
            }
        }
        public string Token = "Dasdqw1273JSK121!au9213kaqL2asdQ21DxWQ";
    }
}
