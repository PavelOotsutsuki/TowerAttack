using UnityEngine;
using Npgsql;
using System; // Для исключений

using Debug = UnityEngine.Debug;

public class DatabaseManager : MonoBehaviour
{
    private string connString;

    void Start()
    {
        // !!! ЗАМЕНИ ДАННЫЕ НА СВОИ (Username и Password) !!!
        connString = "Host=host;Port=1234;Username=idinahui;Password=dopboeb;Database=pasholNahuiDB";

        // Тестируем функционал
        WriteData("Unity_Герой", 500);
        ReadData("Unity_Герой");
    }

    /// <summary>
    /// Добавляет нового игрока или обновляет его счёт, если имя уже занято
    /// </summary>
    public void WriteData(string playerName, int newScore)
    {
        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                Debug.Log("✅ Подключено к БД для записи");

                // UPSERT (INSERT, но если конфликт по имени - обновить счёт)
                string sql = @"
                    INSERT INTO players (username, score, level) 
                    VALUES (@username, @score, @level)
                    ON CONFLICT (username) 
                    DO UPDATE SET score = EXCLUDED.score, last_login = CURRENT_TIMESTAMP;
                ";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    // Параметры защищают от SQL-инъекций
                    cmd.Parameters.AddWithValue("username", playerName);
                    cmd.Parameters.AddWithValue("score", newScore);
                    cmd.Parameters.AddWithValue("level", 1); // Пока всем ставим 1-й уровень

                    int rowsAffected = cmd.ExecuteNonQuery();
                    Debug.Log($"📝 Затронуто строк в БД: {rowsAffected}. Игрок: {playerName}, Счёт: {newScore}");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"❌ Ошибка записи в БД: {ex.Message}");
        }
    }

    /// <summary>
    /// Читает данные конкретного игрока по имени
    /// </summary>
    public void ReadData(string playerName)
    {
        try
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                string sql = "SELECT username, score, level FROM players WHERE username = @username";

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("username", playerName);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string name = reader.GetString(0);
                            int score = reader.GetInt32(1);
                            int level = reader.GetInt32(2);

                            Debug.Log($"📖 Найден игрок: {name} | Счёт: {score} | Уровень: {level}");
                        }
                        else
                        {
                            Debug.LogWarning($"⚠️ Игрок с именем '{playerName}' не найден в базе.");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"❌ Ошибка чтения из БД: {ex.Message}");
        }
    }
}