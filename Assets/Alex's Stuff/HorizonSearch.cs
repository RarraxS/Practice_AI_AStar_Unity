using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.Analytics;

public class HorizonSearch : MonoBehaviour
{
//    Función HorizonSearch(inicio, objetivo, horizonte_maximo):
//    Inicializar frontera como una cola vacía
//    Inicializar explorados como un conjunto vacío
//    Agregar el estado inicial a la frontera

//    Mientras la frontera no esté vacía:
//        estado_actual = extraer de la frontera el primer elemento
//        Si estado_actual es igual a objetivo:
//            devolver "Objetivo encontrado"
        
//        Si la profundidad de estado_actual es mayor que horizonte_maximo:
//            continuar con el siguiente ciclo(ignorar este estado)


//        Si estado_actual no está en explorados:
//            Agregar estado_actual a explorados
//            Generar los sucesores de estado_actual

//            Para cada sucesor:
//                Si no está en explorados:
//                    Agregar sucesor a la frontera
//    Fin Mientras

//    devolver "Objetivo no encontrado dentro del horizonte"
//Fin Función

//   Inicio y Objetivo: El algoritmo comienza con el estado inicial y tiene como objetivo encontrar un estado objetivo.
//   Frontera: Usa una cola para manejar los estados que aún no se han explorado.La frontera es donde se almacenan los estados por explorar.
//   Explorados: Un conjunto que mantiene los estados que ya se han explorado para evitar ciclos.
//   Búsqueda por Horizonte: Limita la exploración de estados hasta un cierto nivel o "horizonte máximo" para evitar la exploración excesiva en espacios muy grandes.
//   Generación de Sucesores: A partir del estado actual, genera los posibles sucesores (estados alcanzables) y los agrega a la frontera si no han sido explorados previamente.
}
