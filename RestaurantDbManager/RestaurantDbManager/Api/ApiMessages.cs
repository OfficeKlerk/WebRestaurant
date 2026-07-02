namespace RestaurantDbManager.Api;

//типы, описывающие сообщения api

//обычное сообщение
public record StringMessage(string Message);

//сообщение с ошибкой
public record ErrorMessage(string Type, string Message);