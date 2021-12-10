using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Windows.Forms;


namespace NV_CLIENT
{
    class NV_BUS
    {
        static IFirebaseConfig config = new FirebaseConfig
        {BasePath = "https://antran259820-default-rtdb.asia-southeast1.firebasedatabase.app/" };  
        static FirebaseClient client = new FirebaseClient (config);

        public async void ListenFirebase(DataGridView dataGridView1)
        {
            EventStreamResponse response = await client.OnAsync("nhanviens", 
                added: (sender, args, context) => { UpdateDataGridView(dataGridView1); },
                changed: (sender, args, context) => { UpdateDataGridView(dataGridView1); },
                removed: (sender, args, context) => { UpdateDataGridView(dataGridView1); }
            );
        }

        private void UpdateDataGridView(DataGridView dataGridView1)
        {
            List<ChitietNV> nhanviens = laydanhsach();
            dataGridView1.BeginInvoke(new MethodInvoker(delegate {
                dataGridView1.DataSource = nhanviens;
            }));

        }

            public List<ChitietNV> laydanhsach()
        {
            FirebaseResponse response = client.Get("nhanviens");
            Dictionary<String, ChitietNV> dictNhanviens = response.ResultAs<Dictionary<String,ChitietNV>>();
            List<ChitietNV> nhanviens = dictNhanviens.Values.ToList();
            return nhanviens;
        }

        public ChitietNV Getdetails(int manv)
        {
            FirebaseResponse response = client.Get("nhanviens/NV" + manv);
            ChitietNV nhanvien = response.ResultAs<ChitietNV>();
            return nhanvien;
        }

        private String GetKeyByCode(int manv)
        {
            FirebaseResponse response = client.Get("nhanviens");
            Dictionary<String, ChitietNV> dictNhanviens = response.ResultAs<Dictionary<String, ChitietNV>>();
            String key = dictNhanviens.FirstOrDefault(x => x.Value.Manv == manv).Key;
            return key;
        }

        public List<ChitietNV> Search(String keyword)
        {
            List<ChitietNV> nhanviens = new List<ChitietNV>();
            foreach(var item in laydanhsach())
            {
                if (item.Ten.ToLower().Contains(keyword.ToLower()))
                {
                    nhanviens.Add(item);
                }
            }
            return nhanviens;
        }

        public bool Addnew(ChitietNV newnhanvien)
        {
            try
            {
                //client.Push("nhanviens", newnhanvien);
                client.Set("nhanviens/NV" + newnhanvien.Manv, newnhanvien); //custom key 
                return true;
            }
            catch { return false; }
        }

        public bool Update(ChitietNV newnhanvien)
        {
            try
            {
                String key = GetKeyByCode(newnhanvien.Manv);
                if (String.IsNullOrEmpty(key)) return false; client.Set("nhanviens/" + key, newnhanvien);
                return true;
            }
            catch { return false; }
        }

        public bool Delete(int manv)
        {
            try
            {
                String key = GetKeyByCode(manv);
                if (String.IsNullOrEmpty(key)) return false; client.Delete("nhanviens/" + key);
                return true;
            }
            catch { return false; }
        }
    }
}
