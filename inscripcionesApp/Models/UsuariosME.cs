﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InscripcionesApp.Models
{
    [Table("USUARIOS")]
    public class UsuariosME
    {
        [Key]
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        [Column("EMAIL")]
        public string? Email { get; set; }

        [Column("PASS")]
        public byte[]? Password { get; set; }

        [Column("SALT")]
        public string? Salt { get; set; }

        [Column("NOMBRE")]
        public string? Nombre { get; set; }

        [Column("APELLIDOS")]
        public string? Apellidos { get; set; }

        [Column("TIPO")]
        public string? Tipo { get; set; }
    }
}
