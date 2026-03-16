namespace ProjectHealthAPI.Models
{
    //Seccion Clientes
    public enum Genero
    {
        Hombre,
        Mujer,
        Otro
    }

    public enum EstadoCivil
    {
        Soltero,
        Casado,
        Divorciado,
        Viudo
    }

    public enum TipoCliente
    {
        Paciente,
        Arrendatario
    }

    public enum EstadoCliente
    {
        Pendiente,
        Cancelado
    }

    //Seccion Servicios
    public enum TipoServicio
    {
        Alquiler,
        Cita
    }

    public enum EstadoServicio
    {
        Pendiente,
        Cancelado
    }

    //Seccion Historias Medicas
    public enum AntecedentesPersonales
    {
        Colesterol,
        Reflujo,
        Estreñimiento,
        Gastritis,
        Artritis,
        Cancer,
        HTA,
        Diabetes
    }

    public enum Fumador
    {
        Frecuente,
        Ocacional,
        Nunca
    }

    public enum ConsumoAlcohol
    {
        Frecuente,
        Ocacional,
        Nunca
    }

    public enum Ejercicio
    {
        Frecuente,
        Ocacional,
        Nunca
    }

    public enum EscalaDolor
    {
        Uno, //verde
        Dos, //verde
        Tres, //verde
        Cuatro, //amarillo
        Cinco, //amarillo
        Seis, //amarillo
        Siete, //naranja
        Ocho, //naranja
        Nueve, //rojo
        Diez //rojo
    }

    //Seccion Pagos
    public enum TipoPago
    {
        Efectivo, //VES
        Binance,
        PagoMovil, //VES
        Divisa //USD
    }

    public enum EstadoPago
    {
        Pendiente,
        Cancelado
    }
}