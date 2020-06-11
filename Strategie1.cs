using System;
using Gefangenendilemma.Basis;

namespace Gefangenendilemma
{
    /// <summary>
    /// Saschas Strategie
    /// Wählt frei nach Zufall aus
    /// </summary>
    public class Strategie1 : BasisStrategie
    {
        public int schwere;

        /// <summary>
        /// Gibt den Namen der Strategie zurück, wichtig zum Anzeigen für die Auswahl
        /// </summary>
        /// <returns></returns>
        public override string Name()
        {
            return "RandyRandom";
        }

        /// <summary>
        /// Gibt den Namen des Autors der Strategie zurück, wichtig für die Turnierpart um den Sieger zu ermitteln.
        /// </summary>
        /// <returns></returns>
        public override string Autor()
        {
            return "Sascha";
        }

        /// <summary>
        /// Teilt mit, dass ein Verhoer jetzt startet
        /// </summary>
        /// <param name="runde">Anzahl der Runden, die verhört wird</param>
        /// <param name="schwere">Schwere des Verbrechen (VLeicht = 0, VMittel = 1, VSchwer = 2)</param>
        public override void Start(int runde, int schwere)
        {
            this.schwere = schwere;
        }

        /// <summary>
        /// Verhoert einen Gefangenen
        /// </summary>
        /// <param name="letzteReaktion">Reaktion des anderen Gefangenen, die Runde davor (NochNichtVerhoert = -1, Kooperieren = 0, Verrat = 1)</param>
        /// <returns>Gibt die eigene Reaktion für diese Runde zurück (Kooperieren = 0, Verrat = 1)</returns>
        public override int Verhoer(int letzteReaktion)
        {
            if(this.schwere == 1)
            {
                return 1;
            }
            Random rnd = new Random();
            int randNr = rnd.Next(0, 2);
            return randNr;
        }
    }
}