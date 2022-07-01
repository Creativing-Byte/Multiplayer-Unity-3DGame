// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("QywMuEZSHTloiv8jg9snkAjl318i9R49jIwtwk4EzDkjM73hIdmYLL8qLa7qdayIBK86oT/6+N7oLZTYqiZV+hz/IyPAzGZr0/YwKijvUIE6DFSufy1FNZnErNk1BwkZJccvkMgJSrFtd53Vv5jLSbOw3zJMU9W3Qc6Ow3gegR5VJgGUFBqtrJHuz+t2qAJKmJeEQVSonJqxhILeIoupMrQGhaa0iYKNrgLMAnOJhYWFgYSHdgCG7PL5aJ0njJnzhySJNcNdNz6QDZXUNu/sfVVaeeTwIuxd6wg3XAaFi4S0BoWOhgaFhYQv4V4CXkJgydgZqXFWOGdq9omvjdAGhBJSw94DBNXND8NRVquDgHkAd+YfQrrSRgjA+KGhHGies4aHhYSF");
        private static int[] order = new int[] { 8,10,4,13,7,7,6,12,8,11,11,11,12,13,14 };
        private static int key = 132;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
