using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using bankomat.Karty;
using Newtonsoft.Json;

namespace bankomat
{
    public class OdczytPlikow
    {
        ////public List<Karta> bankomatowe { get; set;}
        public List<KartaJunior> Jbankomatowe { get; set; }
        public List<KartaBankomatowa> Bbankomatowe { get; set; }
        public List<KartaKredytowa> Kbankomatowe { get; set; }
        public List<KartaPlatnicza> Pbankomatowe { get; set; }


        public void Odczyt()
        {
            string sciezkajunior = File.ReadAllText(@"C:/bankomat/bazadanych/Juniorowe.json");
            Jbankomatowe = JsonConvert.DeserializeObject<List<KartaJunior>>(sciezkajunior);

            string sciezkabankomatowa = File.ReadAllText(@"C:/bankomat/bazadanych/Bankomatowe.json");
            Bbankomatowe = JsonConvert.DeserializeObject<List<KartaBankomatowa>>(sciezkabankomatowa);

            string sciezkakredytowa = File.ReadAllText(@"C:/bankomat/bazadanych/Kredytowe.json");
            Kbankomatowe = JsonConvert.DeserializeObject<List<KartaKredytowa>>(sciezkakredytowa);

            string sciekaplatnicza = File.ReadAllText(@"C:/bankomat/bazadanych/Platnicze.json");
            Pbankomatowe = JsonConvert.DeserializeObject<List<KartaPlatnicza>>(sciekaplatnicza);
        }

        
        


    }

}