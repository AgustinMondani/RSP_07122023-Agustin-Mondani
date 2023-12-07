using Entidades.Exceptions;
using Entidades.DataBase;
using Entidades.Files;
using Entidades.Interfaces;


namespace Entidades.Modelos
{
    public delegate void DelegadoDemoraAtencion(double demora);
    public delegate void DelegadoNuevoPedido(IComestible menu);

    public class Cocinero<T> where T : IComestible, new()
    {
        private int cantPedidosFinalizados;
        private string nombre;
        private double demoraPreparacionTotal;
        private CancellationTokenSource cancellation;
        private Mozo<T> mozo;
        private Queue<T> pedidos;

        private T pedidosEnPreparacion;

        public event DelegadoNuevoPedido OnPedido;
        public event DelegadoDemoraAtencion OnDemora;

        private Task tarea;

        public Cocinero(string nombre)
        {
            this.nombre = nombre;
            this.mozo = new Mozo<T>();
            this.pedidos = new Queue<T>();
            this.OnPedido += this.TomarNuevoPedido;
        }

        //No hacer nada
        public bool HabilitarCocina
        {
            get
            {
                return this.tarea is not null && (this.tarea.Status == TaskStatus.Running ||
                    this.tarea.Status == TaskStatus.WaitingToRun ||
                    this.tarea.Status == TaskStatus.WaitingForActivation);
            }
            set
            {
                if (value && !this.HabilitarCocina)
                {
                    this.cancellation = new CancellationTokenSource();
                    this.EmpezarACocinar();
                }
                else
                {
                    this.cancellation.Cancel();
                }
            }
        }

        //no hacer nada
        public double TiempoMedioDePreparacion { get => this.cantPedidosFinalizados == 0 ? 0 : this.demoraPreparacionTotal / this.cantPedidosFinalizados; }
        public string Nombre { get => nombre; }
        public int CantPedidosFinalizados { get => cantPedidosFinalizados; }

        private void EmpezarACocinar()
        {
            tarea = Task.Run(() =>
            {
                while (cancellation.IsCancellationRequested.Equals(false))
                {
                    if(pedidos.Count > 0)
                    {
                        this.pedidosEnPreparacion = pedidos.Dequeue();
                        if (OnPedido is not null)
                        {
                            pedidosEnPreparacion.IniciarPreparacion();
                            OnPedido.Invoke(pedidosEnPreparacion);
                        }
                        EsperarProximoIngreso();
                        cantPedidosFinalizados++;
                        DataBaseManager.GuardarTicket<T>(nombre, pedidosEnPreparacion);
                    }
                }
            }, cancellation.Token);
        }

        private void EsperarProximoIngreso()
        {
            if (OnDemora is not null)
            {
                int tiempoEspera = 0;
                
                while(!pedidosEnPreparacion.Estado && cancellation.IsCancellationRequested.Equals(false))
                {
                    OnDemora.Invoke(tiempoEspera);
                    Thread.Sleep(1000);
                    tiempoEspera++;
                }
                demoraPreparacionTotal += tiempoEspera;
            }
        }

        private void TomarNuevoPedido(T menu)
        {
            if(OnPedido is not null)
            {
                pedidos.Enqueue(menu);
            }
        }
    }
}
