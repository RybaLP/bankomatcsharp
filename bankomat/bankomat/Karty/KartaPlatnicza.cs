using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace bankomat.Karty
{
    public class KartaPlatnicza : Karta
    {
        public bool Odblokowana { get; set; }

        public void Witaj(KartaPlatnicza kartaPlatnicza, List<KartaPlatnicza> Pbankomatowe)
        {
            Console.Clear();
            bool continueOperation = true;

            do
            {
                Console.Clear();
                Console.WriteLine($"Witamy! {ImieWlasciciela} {NazwiskoWlasciciela}");
                Console.WriteLine();
                Console.WriteLine("Wybierz operację: ");
                Console.WriteLine("1 ----- Wpłata na konto");
                Console.WriteLine("2 ----- Wypłata pieniędzy z konta ");
                Console.WriteLine("3 ----- Sprawdz Saldo");
                Console.WriteLine("4 ----- Zamknij aplikację");

                int wyborOperacji;

                if (int.TryParse(Console.ReadLine(), out wyborOperacji))
                {
                    switch (wyborOperacji)
                    {
                        case 1:
                            Wplac(kartaPlatnicza, Pbankomatowe);         
                            break;
                        case 2:
                            Wyplac(kartaPlatnicza, Pbankomatowe);
                            break;
                        case 3:
                            Console.WriteLine($"Twoje obecne saldo wynosi: {Saldo}");
                            break;
                        case 4:
                            continueOperation = false;
                            break;
                        default:
                            Console.WriteLine("Nieoczekiwany błąd");
                            break;
                    }

                    if (continueOperation)
                    {
                        Console.WriteLine("Czy chcesz kontynuować? (T/N)");
                        char decision = Console.ReadKey().KeyChar;
                        continueOperation = (decision == 'T' || decision == 't');
                    }
                }
            } while (continueOperation);
        }

        public void Wplac(KartaPlatnicza kartaPlatnicza, List<KartaPlatnicza> Pbankomatowe)
        {
            try
            {
                Console.WriteLine("Podaj kwotę do wpłaty:");
                if (Odblokowana == false)
                {
                    Console.WriteLine("Karta jest zablokowana!");
                    Console.WriteLine("W celu odblokowaniu karty prosimy udać się do banku ");
                }
                Pbankomatowe.Remove(kartaPlatnicza);
                if (decimal.TryParse(Console.ReadLine(), out decimal kwota) && kwota > 0 && Odblokowana == true)
                {
                    kartaPlatnicza.Saldo += kwota;
                    Console.WriteLine($"Wpłacono {kwota} zł. Nowe saldo: {kartaPlatnicza.Saldo} zł.");
                    Pbankomatowe.Add(kartaPlatnicza);
                    string newJson = JsonConvert.SerializeObject(Pbankomatowe, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(@"C:/bankomat/bazadanych/Platnicze.json", newJson);
                }
                else
                {
                    Console.WriteLine("Nieprawidłowa kwota do wpłaty lub przekroczony limit.");
                }
            }
            catch(Exception ex)
            {
                Pbankomatowe.Add(kartaPlatnicza);
                Console.WriteLine(ex.Message);
            }
           
        }

        public void Wyplac(KartaPlatnicza kartaPlatnicza, List<KartaPlatnicza> Pbankomatowe)
        {
            try
            {
                if (Odblokowana == false)
                {
                    Console.WriteLine("Karta zablokowana! pros");
                }

                Pbankomatowe.Remove(kartaPlatnicza);

                Console.WriteLine("podaj kwote do wplaty");
                int kwota = int.Parse(Console.ReadLine());

                if (kwota > 0 && Odblokowana == true)
                {
                    kartaPlatnicza.Saldo -= (int)kwota;
                    Console.WriteLine($"Wypłacono {kwota} zł. Nowe saldo: {Saldo} zł.");
                    Pbankomatowe.Add(kartaPlatnicza);
                    string newJson = JsonConvert.SerializeObject(Pbankomatowe, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(@"C:/bankomat/bazadanych/Platnicze.json", newJson);
                }
                else
                {
                    Console.WriteLine("Nieprawidłowa kwota do wypłaty lub przekroczony limit.");
                }
            }
                catch (Exception ex)
            {
                Pbankomatowe.Add(kartaPlatnicza);
                Console.WriteLine(ex.Message);
            }
        }

    }
}
