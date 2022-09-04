﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/**
 * 
 * Общие типы
 * Согласно RFC 2045, RFC 2046, RFC 4288, RFC 4289 и RFC 4855[3], 
 * выделяются следующие базовые типы передаваемых данных:    
 * 
    application;       

    audio;
    image;
    video; 

    message;
    text;

    model;
    multipart;
    
    

   Внутренний формат прикладной программы:
        application/atom+xml: Atom
        application/EDI-X12: EDI X12 (RFC 1767)
        application/EDIFACT: EDI EDIFACT (RFC 1767)
        application/json: JavaScript Object Notation JSON (RFC 4627)
        application/javascript: JavaScript (RFC 4329)
        application/octet-stream: двоичный файл без указания формата (RFC 2046)[4]
        application/ogg: Ogg (RFC 5334)
        application/pdf: Portable Document Format, PDF (RFC 3778)
        application/postscript: PostScript (RFC 2046)
        application/soap+xml: SOAP (RFC 3902)
        application/font-woff: Web Open Font Format[5]
        application/xhtml+xml: XHTML (RFC 3236)
        application/xml-dtd: DTD (RFC 3023)
        application/xop+xml:XOP
        application/zip: ZIP[6]
        application/gzip: Gzip
        application/x-bittorrent : BitTorrent
        application/x-tex : TeX
        application/xml: XML
        application/msword: DOC
 

   Аудио
        audio/basic: mulaw аудио, 8 кГц, 1 канал (RFC 2046)
        audio/L24: 24bit Linear PCM аудио, 8-48 кГц, 1-N каналов (RFC 3190)
        audio/mp4: MP4
        audio/aac: AAC
        audio/mpeg: MP3 или др. MPEG (RFC 3003)
        audio/ogg: Ogg Vorbis, Speex, Flac или др. аудио (RFC 5334)
        audio/vorbis: Vorbis (RFC 5215)
        audio/x-ms-wma: Windows Media Audio[7]
        audio/x-ms-wax: Windows Media Audio перенаправление
        audio/vnd.rn-realaudio: RealAudio[8]
        audio/vnd.wave: WAV(RFC 2361)
        audio/webm: WebM
    

   Изображение
        image/gif: GIF(RFC 2045 и RFC 2046)
        image/jpeg: JPEG (RFC 2045 и RFC 2046)
        image/pjpeg: JPEG[9]
        image/png: Portable Network Graphics[10](RFC 2083)
        image/svg+xml: SVG[11]
        image/tiff: TIFF(RFC 3302)
        image/vnd.microsoft.icon: ICO[12]
        image/vnd.wap.wbmp: WBMP
        image/webp: WebP
        
    Сообщение
        message/http (RFC 2616)
        message/imdn+xml: IMDN (RFC 5438)
        message/partial: E-mail (RFC 2045 и RFC 2046)
        message/rfc822: E-mail; EML-файлы, MIME-файлы, MHT-файлы, MHTML-файлы (RFC 2045 и RFC 2046)
        
    Для 3D-моделей
        model/example: (RFC 4735)
        model/iges: IGS файлы, IGES файлы (RFC 2077)
        model/mesh: MSH файлы, MESH файлы (RFC 2077), SILO файлы
        model/vrml: WRL файлы, VRML файлы (RFC 2077)
        model/x3d+binary: X3D ISO стандарт для 3D компьютерной графики, X3DB файлы
        model/x3d+vrml: X3D ISO стандарт для 3D компьютерной графики, X3DV VRML файлы
        model/x3d+xml: X3D ISO стандарт для 3D компьютерной графики, X3D XML файлы

    Смешанный
        multipart
        multipart/mixed: MIME E-mail (RFC 2045 и RFC 2046)
        multipart/alternative: MIME E-mail (RFC 2045 и RFC 2046)
        multipart/related: MIME E-mail (RFC 2387 и используемое MHTML (HTML mail))
        multipart/form-data: MIME Webform (RFC 2388)
        multipart/signed: (RFC 1847)
        multipart/encrypted: (RFC 1847)
    
    Текст
        text/cmd: команды
        text/css: Cascading Style Sheets (RFC 2318)
        text/csv: CSV (RFC 4180)
        text/html: HTML (RFC 2854)
        text/javascript (Obsolete): JavaScript (RFC 4329)
        text/plain: текстовые данные (RFC 2046 и RFC 3676)
        text/php: Скрипт языка PHP
        text/xml: Extensible Markup Language (RFC 3023)
        text/markdown: файл языка разметки Markdown (RFC 7763)
        text/cache-manifest: файл манифеста(RFC 2046)
      
    Видео
        video/mpeg: MPEG-1 (RFC 2045 и RFC 2046)
        video/mp4: MP4 (RFC 4337)
        video/ogg: Ogg Theora или другое видео (RFC 5334)
        video/quicktime: QuickTime[13]
        video/webm: WebM
        video/x-ms-wmv: Windows Media Video[7]
        video/x-flv: FLV
        video/x-msvideo: AVI
        video/3gpp: .3gpp .3gp [14]
        video/3gpp2: .3gpp2 .3g2 [14]
         

    Нестандартные файлы

        application/x-www-form-urlencoded Form Encoded Data[19]
        application/x-dvi: DVI
        application/x-latex: LaTeX файлы
        application/x-font-ttf: TrueType (не зарегистрированный MIME-тип, но наиболее часто используемый)
        application/x-shockwave-flash: Adobe Flash[20] и[21]
        application/x-stuffit: StuffIt
        application/x-rar-compressed: RAR
        application/x-tar: Tarball
        text/x-jquery-tmpl: jQuery
        application/x-javascript:
       
    PKCS
        application/x-pkcs12: p12 файлы
        application/x-pkcs12: pfx файлы
        application/x-pkcs7-certificates: p7b файлы
        application/x-pkcs7-certificates: spc файлы
        application/x-pkcs7-certreqresp: p7r файлы
        application/x-pkcs7-mime: p7c файлы
        application/x-pkcs7-mime: p7m файлы
        application/x-pkcs7-signature: p7s файлы
 */
public class FilesModule
{
    public FilesModule(string name)
    {

    }
}

