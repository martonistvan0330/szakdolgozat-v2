package hu.bme.aut.android.homeworkmanagerapp

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.navigation.compose.rememberNavController
import hu.bme.aut.android.homeworkmanagerapp.navigation.NavGraph
import hu.bme.aut.android.homeworkmanagerapp.ui.theme.HomeworkManagerAppTheme

//@AndroidEntryPoint
class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            HomeworkManagerAppTheme {
                val navController = rememberNavController()
                NavGraph(navController = navController)
            }
        }
    }
}