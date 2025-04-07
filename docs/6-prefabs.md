# Prefabs

## AstroCat (Jogador)

- **Descrição:**  
  Personagem controlável principal do jogo.

- **Quando são utilizados:**  
  Presente durante todo o gameplay.

- **Componentes:**  
  - Sprite Renderer (AstroCat animado em diferentes estados)  
  - Rigidbody2D (movimentação com física)  
  - Collider2D (detecção de colisões)  
  - Audio Source (sons de passos, pulo, dano, etc.)  
  - Scripts:  
    - **PlayerController.cs** – controla movimentação, pulo, dash, animações  
    - **HealthManager.cs** – gerencia vida e dano  
    - **AbilityUnlock.cs** – ativa novas habilidades quando obtidas

---

## Plataforma Quebrável

- **Descrição:**  
  Plataforma que se desfaz quando o jogador pisa.

- **Quando são utilizadas:**  
  Em áreas que exigem timing ou que limitam acesso a certos caminhos.

- **Componentes:**  
  - Sprite Renderer (plataforma rachada)  
  - Collider2D (ativa a colisão com o jogador)  
  - Audio Source (som de quebra)  
  - Scripts:  
    - **BreakablePlatform.cs** – detecta o jogador e inicia um timer para desaparecer, depois se regenera ou desaparece permanentemente

---

## Inimigo Básico

- **Descrição:**  
  Criatura alienígena hostil que patrulha uma área.

- **Quando são utilizados:**  
  Em trechos onde se quer aumentar a dificuldade de movimentação.

- **Componentes:**  
  - Sprite Renderer (inimigo animado)  
  - Rigidbody2D  
  - Collider2D  
  - Audio Source (som de ataque, dano)  
  - Scripts:  
    - **EnemyPatrol.cs** – movimento entre dois pontos definidos  
    - **EnemyDamage.cs** – causa dano ao jogador ao tocar e reage ao ser derrotado

---

## NPC Alienígena

- **Descrição:**  
  Personagem que interage com o jogador, oferecendo dicas ou itens.

- **Quando são utilizados:**  
  Em pontos estratégicos de progressão ou narrativa.

- **Componentes:**  
  - Sprite Renderer (alienígena com animações)  
  - Collider2D (detecção de aproximação)  
  - Audio Source (efeitos sonoros de fala)  
  - Scripts:  
    - **NPCDialogue.cs** – exibe diálogo ao interagir  
    - **GiveItem.cs** – entrega itens ou ativa eventos se certas condições forem atendidas

---

## Peça da Nave

- **Descrição:**  
  Item essencial para concluir o jogo.

- **Quando são utilizadas:**  
  Espalhadas pelo mapa em locais-chave.

- **Componentes:**  
  - Sprite Renderer (peça brilhante)  
  - Collider2D (trigger)  
  - Audio Source (som de coleta)  
  - Scripts:  
    - **CollectiblePiece.cs** – adiciona a peça ao inventário e marca como coletada
