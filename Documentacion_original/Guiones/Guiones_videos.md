# Tarea 4: Guiones de los tres videos

UNIDAD 4  /  ESTRUCTURAS NO LINEALES

Demostración, explicación y requisitos de publicación

| Datos académicos | Información |
| --- | --- |
| Alumno | [Nombre completo del alumno] |
| Matrícula y grupo | [Matrícula] / [Grupo] |
| Institución | [Institución] |
| Asignatura | [Asignatura] |
| Docente | [Docente] |
| Fecha | 18 de noviembre de 2026 |

Competencia: comprende y aplica estructuras no lineales para la solución de problemas.

## Preparación común

Esta guía sirve para grabar tus propios videos. No contiene videos ya grabados ni enlaces publicados. Completa tu nombre y datos académicos; ensaya con el programa y verifica que puedes explicar cada operación sin limitarte a leer.

| Elemento | Acción |
| --- | --- |
| Pantalla | Mostrar PDF y programa con texto legible; hacer zoom cuando se vea código o una tabla. |
| Cámara | Colocar la cámara web visible en la esquina inferior derecha; evitar que cubra recorridos o resultados. |
| Audio | Grabar una prueba breve y comprobar voz y volumen antes de empezar. |
| Canal | Publicar los tres videos en el mismo canal del alumno, también el usado en las unidades anteriores. |
| Visibilidad | La consigna exige Público; comprobar el enlace sin iniciar sesión. |
| Evidencias | Mostrar tu ejecución. Las evidencias incluidas corresponden al motor C# ejecutado en Linux. |

Duración orientativa: 7 a 10 minutos por tarea. Los tiempos no son un requisito indicado por el docente. Mantén tu cámara visible durante toda la explicación y nombra el objetivo de cada demostración.

## Video 4.1: árboles y grafos

### 0:00-0:40 | Presentación

“Soy [nombre]. Explicaré cómo se clasifican los árboles y cómo se representan los grafos. La elección depende de las relaciones que se quieren modelar y de las operaciones que se van a realizar.”

### 0:40-2:40 | Clasificación

Muestra el organizador de árboles. Explica binario, ABB, AVL y B/B+. Aclara que los criterios se superponen: un AVL es un ABB binario. Compara un heap con un ABB y menciona una aplicación para cada familia. No afirmes que todo árbol binario está ordenado o balanceado.

### 2:40-4:00 | Terminología

Muestra el árbol del ejemplo. Señala raíz 50; padre de 65: 60; hojas 20,40,65,80; altura 3 y nivel de la raíz 0. Diferencia altura y profundidad. Indica que 60 tiene un solo hijo, de modo que no es un árbol lleno.

### 4:00-6:00 | Grafo y representaciones

Muestra el mapa de grafos y la tabla matriz/lista. Explica dirección y peso como propiedades independientes. Usa la red A-B con peso 4 y B-C con peso 0 para mostrar por qué ausencia no puede representarse siempre con cero.

### 6:00-8:00 | Elección y conclusión

“La matriz consulta una arista en O(1), pero reserva O(V²). La lista ahorra espacio en redes dispersas y permite recorrer vecinos. Para el proyecto usaré un ABB para claves y un grafo ponderado para rutas.” Explica por qué Dijkstra y el Problema del Viajero resuelven objetivos diferentes.

### Preguntas de preparación

¿Qué diferencia hay entre completo y perfecto? ¿Un AVL puede ser también ABB? ¿Por qué un heap no permite buscar cualquier clave como un ABB? ¿Por qué una arista de costo cero no equivale a ausencia?

## Video 4.2: ABB dinámico

### 0:00-1:10 | Objetivo y estructura

Abre TreeNode.cs y BinarySearchTree.cs. Explica Key, Left, Right, Root y Count. “Cada nodo se crea dinámicamente. Las claves son únicas: el subárbol izquierdo contiene menores y el derecho mayores.”

### 1:10-3:00 | Inserción y búsqueda

Abre la pestaña ABB y pulsa Ejemplo. Muestra 50,30,70,20,40,60,80,65. Busca 65 y describe el recorrido 50→70→60→65. Busca 99 e inserta 50 de nuevo para demostrar el rechazo del duplicado sin cambiar Count.

### 3:00-4:30 | Recorridos

Lee las reglas de preorden, inorden y postorden; señala dónde aparece 50 en cada resultado. “La pila explícita permite recorrer en profundidad sin llamadas recursivas. Inorden es creciente porque se cumple la propiedad ABB.”

### 4:30-6:40 | Eliminación

Elimina 20, luego 60, luego 70 y finalmente 50. Explica hoja, un hijo y dos hijos. En el último paso la nueva clave de la raíz es 65. Compara los tres recorridos finales con el reporte y guarda el registro de tu ejecución.

### 6:40-8:20 | Pruebas y límites

Ejecuta el comando test en la consola y muestra el resultado. Explica que se compara con SortedSet y recorridos independientes. Aclara que 45,289 es el total de aserciones de ABB y grafos. “No implementé balanceo automático; con claves ascendentes el ABB se vuelve una cadena y la operación puede costar O(n).”

### Preguntas de preparación

¿Por qué el sucesor nunca tiene hijo izquierdo? ¿Qué ocurre si tiene hijo derecho? ¿Por qué Count baja solo una vez al borrar un nodo con dos hijos? ¿Por qué altura 2999 no significa 2999 nodos?

## Video 4.3: grafo y Dijkstra

### 0:00-1:00 | Problema

“Modelo seis puntos de distribución y nueve enlaces bidireccionales. Los kilómetros son hipotéticos. Busco el camino de A a F de menor suma de pesos, sin obligación de visitar todos los vértices.”

### 1:00-2:20 | Representaciones

Carga el ejemplo y muestra lista y matriz. Señala que son el mismo grafo y que la matriz es simétrica. Explica null/ausencia y el cero diagonal. Enseña WeightedGraph.cs: cada enlace se registra en ambas listas.

### 2:20-5:20 | Traza

Selecciona origen A y destino F y pulsa Calcular. Recorre A,C,B,D,E,F en la tabla. Explica cómo B baja de 4 a 3 por C; D de 10 a 8 por B; F de 14 a 13 por E. Describe una relajación con la suma completa.

### 5:20-6:30 | Ruta y comparación

Reconstruye desde F usando predecesores: F,E,D,B,C,A. Invierte para obtener A-C-B-D-E-F. Suma 2+1+5+2+3=13. Compara con A-B-D-F=15 y calcula reducción de 2 km, 13.33% frente a esa ruta declarada.

### 6:30-7:40 | Casos límite

Escribe vértices A,B,C y solo la arista A B 0; carga la red. Consulta C desde A: inalcanzable. Consulta B: costo 0. Prueba peso -1 y muestra el rechazo. Restaura el ejemplo al terminar y guarda el resultado.

### 7:40-9:20 | Verificación y conclusión

Explica que 120 grafos aleatorios se contrastaron con Floyd-Warshall para todas las fuentes. “Mi Dijkstra selecciona el mínimo con un recorrido lineal: O(V²+E). No usa heap. La traza conserva copias y requiere memoria adicional. El resultado optimiza este modelo, no una red vial real.”

### Preguntas de preparación

¿Por qué no se admiten pesos negativos? ¿En qué difiere Dijkstra de BFS? ¿Cómo se detecta un destino inalcanzable? ¿El camino que visita todos los puntos del ejemplo prueba que se resolvió el Viajero?

## Lista final de entrega

| Tarea | Material preparado | Acción pendiente del alumno |
| --- | --- | --- |
| 4.1 | PDF con dos organizadores vectoriales, contexto y referencias; SVG/PDF de diagramas. | Completar portada y grabar/publicar video 4.1. |
| 4.2 | Clase árbol y nodo, formulario, reporte y pruebas reales del motor. | Probar interfaz Windows; grabar/publicar video 4.2. |
| 4.3 | Solución comprimida, aplicación, matriz/lista, traza y reporte formal. | Grabar/publicar video 4.3 y completar enlaces. |

Publica el repositorio desde tu cuenta si se solicita el enlace. Agrega la URL real y los tres videos a datos_portada.json. Regenera los PDF con python generar_reportes.py; vuelve a comprimir los archivos actualizados. No sustituyas los campos pendientes por enlaces inventados.

Para abrir: descomprime toda la carpeta y conserva juntos los archivos de ejecutable_windows. El ejecutable Windows x64 requiere .NET Desktop Runtime 10. Para editar se entrega Unidad4.sln y los archivos MainForm.cs, MainForm.Designer.cs y MainForm.resx.

La compilación y las pruebas del motor ya se realizaron. La ejecución visual en Windows y las grabaciones con la cámara del alumno permanecen pendientes. Los guiones son una base para explicar con tus propias palabras, no una evidencia de que los videos ya existen.
