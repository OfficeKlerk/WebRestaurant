# 🍽️ Тбилисский дворик — сайт ресторана грузинской кухни

Дипломный проект: веб-сайт ресторана с онлайн-меню и корзиной заказа, backend-ом на ASP.NET Core Web API и БД на MS SQL Server.

📄 [Презентация проекта](./Презентация%20дп.pdf)

## Содержание

- [Архитектура](#архитектура)
- [Стек технологий](#стек-технологий)
- [Структура репозитория](#структура-репозитория)
- [Функциональность](#функциональность)
- [API](#api)
- [Модель данных](#модель-данных)
- [Быстрый старт](#быстрый-старт)

## Архитектура

```
┌──────────────────────┐      HTTP/JSON      ┌───────────────────────┐      EF Core      ┌─────────────────┐
│   restaurant web-site │ ───────────────────▶│   RestaurantDbManager  │ ──────────────────▶│  MS SQL Server   │
│  (HTML / CSS / JS)    │◀─────────────────────│   (ASP.NET Core API)  │◀────────────────────│  (Docker)        │
└──────────────────────┘                      └───────────────────────┘                    └─────────────────┘

                    ┌──────────────────────────────┐
                    │  AddMenuPositionImagesCript   │  — консольная утилита для массовой
                    │  (.NET console app)           │    загрузки фотографий блюд в БД через API
                    └──────────────────────────────┘
```

## Стек технологий

**Backend**
- ASP.NET Core Web API (.NET 8)
- Entity Framework Core 8 + SQL Server провайдер
- Swagger / Swashbuckle (документация API)
- MS SQL Server 2022 (в Docker)

**Frontend**
- HTML5, CSS3, vanilla JavaScript (без фреймворков)

**Вспомогательные инструменты**
- Консольное приложение на .NET 8 для пакетной загрузки изображений блюд (SixLabors.ImageSharp)

## Структура репозитория

```
.
├── restaurant web-site/          # Клиентская часть (статический сайт)
│   ├── index/                    # Главная страница
│   ├── menu-category-page/       # Страница категории меню
│   ├── menu-position-page/       # Страница блюда
│   └── cart-page/                # Корзина заказа
│
├── RestaurantDbManager/          # Backend: ASP.NET Core Web API
│   └── RestaurantDbManager/
│       ├── Api/Controllers/      # REST-контроллеры
│       ├── Model/                # Репозитории и сценарии (бизнес-логика)
│       └── Rdb/                  # EF Core: сущности, миграции, хранилища
│
├── AddMenuPositionImagesCript/   # Консольная утилита загрузки фото блюд в БД
│
├── docker/
│   └── docker-compose.yml        # Поднятие MS SQL Server в контейнере
│
└── Презентация дп.pdf            # Презентация к диплому
```

## Функциональность

- Просмотр меню ресторана по категориям
- Карточка блюда с фотографией, описанием, весом и ценой
- Корзина заказа с возможностью оформления
- REST API для управления категориями, блюдами, фотографиями и заказами
- Хранение фотографий блюд и категорий прямо в БД (`byte[]`)

## API

Backend поднимается со Swagger UI для интерактивной документации. Основные группы эндпоинтов:

| Контроллер | Маршрут | Описание |
|---|---|---|
| `RootController` | `GET /api/root`, `GET /api/root/ping` | Проверка работоспособности сервера |
| `MenuCategoryController` | `GET/POST/PUT/DELETE /api/menu-categories` | Категории меню |
| `MenuPositionController` | `GET/POST/PUT/DELETE /api/menu-positions` | Позиции (блюда) меню |
| `MenuPositionImageController` | `GET/POST/PUT/DELETE /api/menu-position-images` | Фотографии блюд |
| `OrderController` | `GET/POST/PUT/DELETE /api/orders` | Заказы |
| `MenuPositionOrderController` | `GET/POST/PUT/DELETE /api/menu-position-orders` | Состав заказа (блюдо ↔ заказ) |

## Модель данных

```
MenuCategory ──1─▶─N── MenuPosition ──1─▶─N── MenuPositionImage
                             │
                             │ N
                             ▼
                      MenuPositionOrder ──N─◀─1── Order
```

- **MenuCategory** — категория меню (название, изображение)
- **MenuPosition** — блюдо (название, цена, описание, вес, категория)
- **MenuPositionImage** — фотографии блюда
- **Order** — заказ (дата, телефон клиента)
- **MenuPositionOrder** — связь заказа с блюдами и их количеством

## Быстрый старт

### 1. Поднять базу данных

```bash
cd docker
docker compose up -d
```

Поднимет MS SQL Server 2022 на `localhost:1433` (логин `sa`, пароль указан в `docker-compose.yml`).

### 2. Запустить backend

```bash
cd RestaurantDbManager/RestaurantDbManager
dotnet ef database update   # применить миграции
dotnet run
```

Строка подключения к БД задаётся в `appsettings.json`. Swagger UI будет доступен по адресу, который выведет `dotnet run` (обычно `https://localhost:{port}/swagger`).

### 3. Открыть frontend

Открыть `restaurant web-site/index/index.html` в браузере (или раздать папку через любой статический веб-сервер).

### 4. (опционально) Загрузить фотографии блюд

```bash
cd AddMenuPositionImagesCript/TestApp
# положить изображения в папку images/ (имена файлов — числа, соответствующие ID блюда)
dotnet run
```

---

> Дипломный проект выполнен в рамках обучения в «Академии топ обучение».
