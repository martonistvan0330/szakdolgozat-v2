package hu.bme.aut.android.homeworkmanagerapp

import android.os.Bundle
import androidx.activity.compose.setContent
import androidx.appcompat.app.AppCompatActivity
import androidx.navigation.compose.rememberNavController
import hu.bme.aut.android.homeworkmanagerapp.navigation.NavGraph
import hu.bme.aut.android.homeworkmanagerapp.ui.theme.HomeworkManagerAppTheme

//@AndroidEntryPoint
class MainActivity : AppCompatActivity() {
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