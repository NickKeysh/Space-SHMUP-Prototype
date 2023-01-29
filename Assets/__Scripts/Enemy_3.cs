using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3 : Enemy
{
    [Header("Set in Inspector: Enemy_3")]
    public float        lifeTime = 5;

    [Header("Set Dynamically: Enemy_3")]
    public float        birthTime;
    public Vector3[]    points;
    
    void Start()
    {
        points = new Vector3[3]; // Инициализировать массив точек

        // Начальная позиция уже определена в Main.SpawnEnemy()
        points[0] = pos;

        // Установить xMin и xMax так же, как это делает Main.SpawnEnemy()
        float xMin = -bndCheck.camWidth + bndCheck.radius;
        float xMax = bndCheck.camWidth - bndCheck.radius;
        
        Vector3 v;
        // Случайно выбрать среднюю точку ниже нижней границы экрана
        v = Vector3.zero;
        v.x = Random.Range(xMin, xMax);
        v.y = -bndCheck.camHeight * Random.Range(2.75f, 2);
        points[1] = v;

        // Случайно выбрать конечную точку выше верхней границы экрана
        v = Vector3.zero;
        v.y = pos.y;
        v.x = Random.Range(xMin, xMax);
        points[2] = v;

        // Записать в birthTime текущее время
        birthTime = Time.time;
    }

    public override void Move()
    {
        // Кривые Безье вычисляются на основе значения u между 0 и 1
        float u = (Time.time - birthTime) / lifeTime;

        if (u > 1) {
            // Это экземпляр Enemy_2 завершил свой жизненный цикл
            Destroy(gameObject);
            return;
        }

        // Интерполировать кривую Безье по трем точкам
        Vector3 p01, p12;
        u = u - 0.2f*Mathf.Sin(u * Mathf.PI * 2); // Сгладить кривую Безье
        p01 = (1-u)*points[0] + u*points[1];
        p12 = (1-u)*points[1] + u*points[2];
        pos = (1-u)*p01 + u*p12;
    }
}
