begin transaction;

use ListaTelefonica;

create table tb_pessoas(
	id_pessoa int identity(1,1) not null primary key,
	nome varchar(255)
);

create table tb_telefones(
	id_telefone int identity(1,1) not null primary key,
	id_pessoa int not null foreign key references tb_pessoas(id_pessoa),
	tipo_telefone bit not null,
	numero varchar(11) not null
);

commit;