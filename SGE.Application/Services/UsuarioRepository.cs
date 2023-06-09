﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SGE.Application.Bases;
using SGE.Application.Filtro;
using SGE.Application.Interfaces;
using SGE.Application.Pagination;
using SGE.Domain.Dtos.Token;
using SGE.Domain.Dtos.Usuario;
using SGE.Domain.Entities;
using SGE.Infraestructure.Context;
using SGE.Utilities.Codec;
using SGE.Utilities.Static;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace SGE.Application.Services
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private ApplicationDbContext _applicationDbContext;
        private IMapper _mapper;
        private IConfiguration _configuration;

        public UsuarioRepository(ApplicationDbContext applicationDbContext, IMapper mapper, IConfiguration configuration)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<BaseResponse<UsuarioDataDto>> Create(UsuarioCreateDto proveedorDTO)
        {
            BaseResponse<UsuarioDataDto> response = new BaseResponse<UsuarioDataDto>();
            try
            {
                var usuarioDB = _mapper.Map<Usuario>(proveedorDTO);
                usuarioDB.Password = Codec.Encriptar(proveedorDTO.Password);
                await _applicationDbContext.Usuarios.AddAsync(usuarioDB);
                await _applicationDbContext.SaveChangesAsync();
                response.Data = _mapper.Map<UsuarioDataDto>(usuarioDB);
                response.Mensaje = ReplyMessage.MESSAGE_QUERY;
            }
            catch (Exception ex)
            {
                response.IsSucces = false;
                response.Mensaje = ReplyMessage.MESSAGE_FALIED;
                response.innerExeption = ex.Message;
            }
            return response;
        }

        public async Task<BaseResponse<bool>> Delete(int id)
        {
            BaseResponse<bool> response = new BaseResponse<bool>();
            try
            {
                var usuarioDb = await _applicationDbContext.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
                if (usuarioDb is null)
                {
                    response.Data = false;
                    response.Mensaje = ReplyMessage.MESSAGE_QUERY_EMPTY;
                }
                _applicationDbContext.Usuarios.Remove(usuarioDb);
                await _applicationDbContext.SaveChangesAsync();
                response.Data = true;
                response.Mensaje = ReplyMessage.MESSAGE_DELETE;
            }
            catch (Exception ex)
            {
                response.IsSucces = false;
                response.Mensaje = ReplyMessage.MESSAGE_FALIED;
                response.innerExeption = ex.Message;
            }
            return response;
        }



        public async Task<PaginationResponse<BaseResponse<List<UsuarioDataDto>>>> GetAll(UsuarioFilter filters)
        {
            PaginationResponse<BaseResponse<List<UsuarioDataDto>>> response = new PaginationResponse<BaseResponse<List<UsuarioDataDto>>>();
            try
            {
                var usuariosDB = await _applicationDbContext.Usuarios.Where(x=>x.Nombre.ToUpper().Contains(filters.Nombre.ToUpper())).Skip(filters.Desde).Take(filters.CantidadRegistros).ToListAsync()!;
                response.Data = new BaseResponse<List<UsuarioDataDto>>();
                response.Cantidad = await _applicationDbContext.Usuarios.Where(x => x.Nombre.ToUpper().Contains(filters.Nombre.ToUpper())).CountAsync();
                response.Data.Data = _mapper.Map<List<UsuarioDataDto>>(usuariosDB);                
                response.Data.Mensaje = ReplyMessage.MESSAGE_QUERY;
            }
            catch (Exception ex)
            {
                response.Data!.IsSucces = false;
                response.Data.Mensaje = ReplyMessage.MESSAGE_FALIED;
                response.Data.innerExeption = ex.Message;
            }
            return response;

        }

        public async Task<BaseResponse<UsuarioDataDto>> GetById(int id)
        {
            BaseResponse<UsuarioDataDto> response = new BaseResponse<UsuarioDataDto>();
            try
            {
                var usuarioDB = await _applicationDbContext.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
                if (usuarioDB is null)
                {
                    response.IsSucces = false;
                    response.Mensaje = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }
                response.Data = _mapper.Map<UsuarioDataDto>(usuarioDB);
                response.Mensaje = ReplyMessage.MESSAGE_QUERY;
            }
            catch (Exception ex)
            {
                response.IsSucces = false;
                response.Mensaje = ReplyMessage.MESSAGE_FALIED;
                response.innerExeption = ex.Message;
            }
            return response;
        }

        public async Task<BaseResponse<UsuarioDataDto>> GetByIdPass(int id)
        {
            BaseResponse<UsuarioDataDto> response = new BaseResponse<UsuarioDataDto>();
            try
            {
                var usuarioDB = await _applicationDbContext.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
                if (usuarioDB is null)
                {
                    response.IsSucces = false;
                    response.Mensaje = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }
                usuarioDB.Password = Codec.DesEncriptar(usuarioDB.Password);
                response.Data = _mapper.Map<UsuarioDataDto>(usuarioDB);
                response.Mensaje = ReplyMessage.MESSAGE_QUERY;
            }
            catch (Exception ex)
            {
                response.IsSucces = false; 
                response.Mensaje = ReplyMessage.MESSAGE_FALIED;
                response.innerExeption = ex.Message; 
            }
            return response;
        }

        public async Task<BaseResponse<UsuarioDataDto>> Update(UsuarioEditDto usuarioDto, int id)
        {
            BaseResponse<UsuarioDataDto> response = new BaseResponse<UsuarioDataDto>();
            try
            {
                var usuarioDB = await _applicationDbContext.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
                usuarioDB.Nombre = usuarioDto.Nombre;
                usuarioDB.Apellidos = usuarioDto.Apellidos;
                usuarioDB.Email = usuarioDto.Email;
                usuarioDB.Password = Codec.Encriptar(usuarioDto.Password);
                usuarioDB.CodigoClienteNG = usuarioDto.CodigoClienteNG;
                usuarioDB.CodigoClienteNM = usuarioDto.CodigoClienteNM;
                usuarioDB.Estado = usuarioDto.Estado;
                usuarioDB.FechaModificacion = DateTime.Now;
                usuarioDB.Admin = usuarioDto.Admin;
                _applicationDbContext.Usuarios.Update(usuarioDB);
                await _applicationDbContext.SaveChangesAsync();
                response.Data = _mapper.Map<UsuarioDataDto>(usuarioDB);
                response.Mensaje = ReplyMessage.MESSAGE_UPDATE;
            }
            catch (Exception ex)
            {
                response.IsSucces = false;
                response.Mensaje = ReplyMessage.MESSAGE_FALIED;
                response.innerExeption = ex.Message;
            }
            return response;
        }

        public async Task<BaseResponse<string>> GenerateToken(TokenRequestDto tokenRequestDto)
        {
            var response = new BaseResponse<string>();
            var account = await AccountByuserName(tokenRequestDto.email);
            if (account is not null)
            {
                if (Codec.DesEncriptar(account.Password) == tokenRequestDto.password)
                {
                    response.Data = GenerateToken(account);
                    response.Mensaje = ReplyMessage.MESSAGE_TOKEN;
                    return response;
                }
                else
                {
                    response.Mensaje = ReplyMessage.MESSAGE_TOKEN_ERROR;
                    response.IsSucces = false;
                }
            }
            else
            {
                response.IsSucces = false;
                response.Mensaje = ReplyMessage.MESSAGE_TOKEN_ERROR;
            }
            return response;
        }


        public string GenerateToken(Usuario user)
        {
            var securittyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));

            var credencials = new SigningCredentials(securittyKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>() {
                new Claim(JwtRegisteredClaimNames.NameId,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.FamilyName,user.Nombre),
                new Claim(JwtRegisteredClaimNames.GivenName,user.Apellidos),
                new Claim(JwtRegisteredClaimNames.Jti,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,Guid.NewGuid().ToString(),ClaimValueTypes.Integer64),
            };
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(int.Parse(_configuration["Jwt:Expires"])),
                notBefore: DateTime.UtcNow,
                signingCredentials: credencials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Usuario> AccountByuserName(string userName)
        {
            return await _applicationDbContext.Usuarios.FirstOrDefaultAsync(x => x.Nombre == userName);
        }

        public async Task<BaseResponse<UsuarioDataDto>> AccountByuserEmail(string userName)
        {
            BaseResponse<UsuarioDataDto> response = new BaseResponse<UsuarioDataDto>();
            var usuario = await _applicationDbContext.Usuarios.FirstOrDefaultAsync(x => x.Email == userName);

            if (usuario != null)
            {
                response.Data = _mapper.Map<UsuarioDataDto>(usuario);
                response.Mensaje = ReplyMessage.MESSAGE_QUERY;
            }
            else
            {
                response.IsSucces = false;
                response.Mensaje = ReplyMessage.MESSAGE_QUERY_EMPTY;
            }
            return response;
        }
    }
}
