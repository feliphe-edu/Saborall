-- =========================================================
-- BANCO DE DADOS SABORALL
-- Integrantes:
-- Micael
-- Feliphe Eduardo
-- Tatiely Tamarys
-- Laura Carolina
-- =========================================================

-- Criando o banco
CREATE DATABASE IF NOT EXISTS saborall
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;

USE saborall;


-- =========================================================
-- TABELA: USUARIOS
-- =========================================================

CREATE TABLE Usuarios (
    id_usuario INT AUTO_INCREMENT PRIMARY KEY,
    email VARCHAR(100) NOT NULL UNIQUE,
    senha VARCHAR(255) NOT NULL
);


-- =========================================================
-- TABELA: VENDEDORES
-- =========================================================

CREATE TABLE Vendedores (
    id_vendedor INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(100) NOT NULL
);


-- =========================================================
-- TABELA: FORNECEDORES
-- =========================================================

CREATE TABLE Fornecedores (
    id_fornecedor INT AUTO_INCREMENT PRIMARY KEY,
    nome_empresa VARCHAR(100) NOT NULL,
    nome_fantasia VARCHAR(100),
    cnpj VARCHAR(18) NOT NULL UNIQUE,
    endereco VARCHAR(150),
    email VARCHAR(100),
    telefone VARCHAR(20)
);


-- =========================================================
-- TABELA: PRODUTOS
-- =========================================================

CREATE TABLE Produtos (
    id_produto INT AUTO_INCREMENT PRIMARY KEY,
    nome_produto VARCHAR(100) NOT NULL,
    categoria VARCHAR(50),
    preco DECIMAL(10,2) NOT NULL,
    id_fornecedor INT,

    CONSTRAINT fk_produtos_fornecedores
        FOREIGN KEY (id_fornecedor)
        REFERENCES Fornecedores(id_fornecedor)
        ON UPDATE CASCADE
        ON DELETE SET NULL
);


-- =========================================================
-- TABELA: ESTOQUE
-- =========================================================

CREATE TABLE Estoque (
    id_estoque INT AUTO_INCREMENT PRIMARY KEY,
    id_produto INT NOT NULL UNIQUE,
    quantidade_disponivel INT NOT NULL DEFAULT 0,

    CONSTRAINT fk_estoque_produtos
        FOREIGN KEY (id_produto)
        REFERENCES Produtos(id_produto)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);


-- =========================================================
-- TABELA: VENDAS
-- =========================================================

CREATE TABLE Vendas (
    id_venda INT AUTO_INCREMENT PRIMARY KEY,
    data_venda DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    id_vendedor INT NOT NULL,
    valor_total DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    forma_pagamento VARCHAR(30) NOT NULL DEFAULT 'Não informado',

    CONSTRAINT fk_vendas_vendedores
        FOREIGN KEY (id_vendedor)
        REFERENCES Vendedores(id_vendedor)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);


-- =========================================================
-- TABELA: ITENS_VENDA
-- =========================================================

CREATE TABLE ItensVenda (
    id_item_venda INT AUTO_INCREMENT PRIMARY KEY,
    id_venda INT NOT NULL,
    id_produto INT NOT NULL,
    quantidade INT NOT NULL,
    preco_unitario DECIMAL(10,2) NOT NULL,
    subtotal DECIMAL(10,2) NOT NULL,

    CONSTRAINT fk_itens_venda_vendas
        FOREIGN KEY (id_venda)
        REFERENCES Vendas(id_venda)
        ON UPDATE CASCADE
        ON DELETE CASCADE,

    CONSTRAINT fk_itens_venda_produtos
        FOREIGN KEY (id_produto)
        REFERENCES Produtos(id_produto)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);


-- =========================================================
-- DADOS INICIAIS
-- =========================================================

-- USUARIOS
INSERT INTO Usuarios (email, senha) VALUES
('micaelarthur10@gmail.com', '123456'),
('felipheeduardo@gmail.com', '12345678');


-- VENDEDORES
INSERT INTO Vendedores (nome) VALUES
('Tatiely Tamarys'),
('Feliphe Eduardo'),
('Micael Arthur'),
('Laura Carolina');


-- FORNECEDORES
INSERT INTO Fornecedores
(nome_empresa, nome_fantasia, cnpj, endereco, email, telefone)
VALUES
(
    'Distribuidora Gelato LTDA',
    'Gelato',
    '11.111.111/0001-11',
    'Rua das Flores, 120',
    'distribuidoragelado@gmail.com',
    '(69) 4002-8922'
),
(
    'Frutas Tropicais LTDA',
    'Frutas Tropicais',
    '22.222.222/0001-22',
    'Av. Brasil, 300',
    'frutastropicais@gmail.com',
    '(69) 99229-2222'
);


-- PRODUTOS
INSERT INTO Produtos
(nome_produto, categoria, preco, id_fornecedor)
VALUES
('Sorvete de Pitaya', 'Sorvetes de Fruta', 12.00, 2),
('Sorvete de Jabuticaba', 'Sorvetes de Fruta', 11.50, 2),
('Sorvete de Queijo', 'Sorvetes de Creme', 13.00, 1),
('Chocolate Supremo', 'Sorvetes de Creme', 14.00, 1),
('Morango Cremoso', 'Sorvetes de Creme', 13.50, 1),
('Ninho com Nutella', 'Sorvetes de Creme', 15.00, 1);


-- ESTOQUE
INSERT INTO Estoque
(id_produto, quantidade_disponivel)
VALUES
(1, 50),
(2, 40),
(3, 30),
(4, 60),
(5, 55),
(6, 45);


-- =========================================================
-- VENDAS INICIAIS
-- =========================================================

INSERT INTO Vendas
(data_venda, id_vendedor, valor_total, forma_pagamento)
VALUES
('2026-08-10 10:00:00', 1, 42.00, 'PIX'),
('2026-08-10 10:30:00', 2, 27.00, 'Cartão'),
('2026-08-10 11:00:00', 3, 15.00, 'Dinheiro'),
('2026-08-10 11:30:00', 1, 24.00, 'PIX'),
('2026-08-10 12:00:00', 4, 11.50, 'Cartão');


-- ITENS DAS VENDAS


INSERT INTO ItensVenda
(id_venda, id_produto, quantidade, preco_unitario, subtotal)
VALUES
(1, 4, 3, 14.00, 42.00),
(2, 5, 2, 13.50, 27.00),
(3, 6, 1, 15.00, 15.00),
(4, 1, 2, 12.00, 24.00),
(5, 2, 1, 11.50, 11.50);


-- =========================================================
-- CONSULTAS PARA TESTE
-- =========================================================

-- Ver usuários
SELECT * FROM Usuarios;

-- Ver vendedores
SELECT * FROM Vendedores;

-- Ver fornecedores
SELECT * FROM Fornecedores;

-- Ver produtos
SELECT * FROM Produtos;

-- Ver estoque
SELECT * FROM Estoque;

-- Ver vendas
SELECT * FROM Vendas;

-- Ver itens das vendas
SELECT * FROM ItensVenda;


-- =========================================================
-- CONSULTA COMPLETA DE ESTOQUE
-- =========================================================

SELECT
    p.id_produto,
    p.nome_produto,
    p.categoria,
    p.preco,
    e.quantidade_disponivel,
    f.nome_fantasia AS fornecedor
FROM Produtos p
INNER JOIN Estoque e
    ON p.id_produto = e.id_produto
LEFT JOIN Fornecedores f
    ON p.id_fornecedor = f.id_fornecedor;


-- =========================================================
-- CONSULTA COMPLETA DE VENDAS
-- =========================================================

SELECT
    v.id_venda,
    v.data_venda,
    vd.nome AS vendedor,
    p.nome_produto,
    iv.quantidade,
    iv.preco_unitario,
    iv.subtotal,
    v.valor_total
FROM Vendas v
INNER JOIN Vendedores vd
    ON v.id_vendedor = vd.id_vendedor
INNER JOIN ItensVenda iv
    ON v.id_venda = iv.id_venda
INNER JOIN Produtos p
    ON iv.id_produto = p.id_produto
ORDER BY v.id_venda;

