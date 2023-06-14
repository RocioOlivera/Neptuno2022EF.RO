using Neptuno2022EF.Datos.Interfaces;
using Neptuno2022EF.Entidades.Dtos.CtaCte;
using Neptuno2022EF.Entidades.Dtos.Venta;
using Neptuno2022EF.Entidades.Entidades;
using Neptuno2022EF.Entidades.Enums;
using Neptuno2022EF.Ioc;
using Neptuno2022EF.Servicios.Interfaces;
using Neptuno2022EF.Servicios.Servicios;
using Neptuno2022EF.Windows.Helpers;
using Neptuno2022EF.Windows.Helpers.Enum;
using NuevaAppComercial2022.Entidades.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Neptuno2022EF.Windows
{
    public partial class frmDetalleCtaCte : Form
    {
        private readonly IServiciosClientes _serviciosClientes;
        private readonly IServiciosCtasCtes _servicioCtasCtes;
        private readonly IRepositorioCtasCtes _repoCtaCte;
        private readonly IRepositorioVentas _repositorioVentas;
        List<DetalleCtaCteListDto> lista;
        List<CtaCte> listaCta;
        private DetalleCtaCteListDto detalle;
        private Cliente cliente;
        public frmDetalleCtaCte(IServiciosCtasCtes servicioCtasCtes, IServiciosClientes serviciosClientes)
        {
            InitializeComponent();
            _servicioCtasCtes= servicioCtasCtes;
            _serviciosClientes= serviciosClientes;
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (lista != null)
            {
                
                if (lista.Count > 0)
                {
                    
                    FormHelper.MostrarDatosEnGrilla<DetalleCtaCteListDto>(dgvDatos, lista);
                    
                }
            }
        }

        public void SetCtaCte(List<DetalleCtaCteListDto> ctaCteDetalleDto)
        {
            lista = ctaCteDetalleDto;
        }



        private void frmDetalleCtaCte_Load(object sender, EventArgs e)
        {
            //txtDomicilio.Text = detalle.cliente.Direccion.ToString();
            //txtCliente.Text = detalle.cliente.Nombre;
            txtSaldoTotal.Text = lista.Sum(x => x.Debe - x.Haber).ToString();
        }

        private void btnIngresarPago_Click(object sender, EventArgs e)
        {
   
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

        }

        private void txtCliente_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

