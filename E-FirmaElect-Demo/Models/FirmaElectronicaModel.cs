using E_FirmaElect_Demo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_FirmaElect_Demo.Models
{
    public class FirmaElectronicaModel
    {
        public FirmaElectronica_BE oFirmaElectronica { get; set; }
        public Persona_BE oPersona { get; set; }
        public String Resultado { get; set; }
    }
}