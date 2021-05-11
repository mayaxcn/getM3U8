using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace getM3U8
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] tv_list = new string[] { "news", "xxx", "lifestyle", "movies", "local" ,"entertainment" };
            string XH_play_list = "";
            string save_tv_txt_path = AppContext.BaseDirectory + "tv.txt";
            foreach (string tv_cate in tv_list)
            {
                string M3U8_list_url = "https://iptv-org.github.io/iptv/categories/" + tv_cate + ".m3u";
                string play_list = GetResponse(M3U8_list_url);
                //string play_list = File.ReadAllText("news.m3u");
                play_list = play_list.Replace("#EXTM3U", "");
                play_list = play_list.Replace("\n", "");
                play_list = play_list.Replace("#", "#\n");
                string[] play_list_array = play_list.Split('#');
                foreach (string pre_link in play_list_array)
                {
                    if (pre_link.Contains("http"))
                    {
                        string handle_link = pre_link.Replace("\n", "");
                        XH_play_list += handle_link.Substring(handle_link.IndexOf(",") + 1, (handle_link.Length - handle_link.IndexOf(",")) - 1).Replace("http", ",http") + "\n";
                    }
                }
            }
            //Console.WriteLine(XH_play_list);
            File.WriteAllText(save_tv_txt_path, XH_play_list, Encoding.UTF8);
            Console.WriteLine(DateTime.Now.ToString("【MM-dd HH:mm:ss】") + " 已将播放列表保存于" + save_tv_txt_path + "文件中\n");
        }

        private static string GetResponse(string url)
        {
            if (url.StartsWith("https"))
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            }
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("audio/x-mpegurl"));
            HttpResponseMessage response = httpClient.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return null;
        }
    }
}
