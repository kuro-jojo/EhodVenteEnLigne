using System.ComponentModel;

using System.Globalization;

using System.Resources;



namespace EhodVenteEnLigne.Resources

{
    public class SharedResources

    {
        private static ResourceManager resourceMan;

        private static CultureInfo resourceCulture;

        [EditorBrowsable(EditorBrowsableState.Advanced)]

        public static ResourceManager ResourceManager

        {
            get

            {
                if (ReferenceEquals(resourceMan, null))

                {

                    ResourceManager temp = new ResourceManager("EhodVenteEnLigne.Resources.SharedResources", typeof(SharedResources).Assembly);

                    resourceMan = temp;

                }

                return resourceMan;

            }

        }



        [EditorBrowsable(EditorBrowsableState.Advanced)]

        public static CultureInfo Culture

        {

            get { return resourceCulture; }

            set { resourceCulture = value; }

        }


        public static string ErrorMissing

        {
            get { return ResourceManager.GetString("{0} is missing", resourceCulture); }

        }

    }



}