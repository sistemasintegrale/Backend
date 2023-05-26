﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Utilities.Static
{
    public  class ReplyMessage
    {
        public const string MESSAGE_QUERY = "Consulta exitosa";
        public const string MESSAGE_QUERY_EMPTY = "No se encontraron registros";
        public const string MESSAGE_SAVE = "Se registró correctamente";
        public const string MESSAGE_UPDATE = "Se actualizó correctamente";
        public const string MESSAGE_DELETE = "Se eliminó correctamente";
        public const string MESSAGE_EXISTS = "El registro ya existe"; 
        public const string MESSAGE_ACTIVATE= "El registro ha sido activado";
        public const string MESSAGE_TOKEN = "Token generado correctamente";
        public const string MESSAGE_VALIDATE = "Errores de validacion";
        public const string MESSAGE_FALIED = "Operacion fallida";
        public const string MESSAGE_TOKEN_ERROR = "El usuario o contraseña es incorrecta";
    }
}