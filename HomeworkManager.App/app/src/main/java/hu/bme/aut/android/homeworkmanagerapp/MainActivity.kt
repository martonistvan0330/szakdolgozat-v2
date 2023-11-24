package hu.bme.aut.android.homeworkmanagerapp

import android.os.Bundle
import androidx.activity.compose.setContent
import androidx.appcompat.app.AppCompatActivity
import dagger.hilt.android.AndroidEntryPoint
import hu.bme.aut.android.homeworkmanagerapp.feature.HomeworkManagerApp
import hu.bme.aut.android.homeworkmanagerapp.ui.theme.HomeworkManagerAppTheme

@AndroidEntryPoint
class MainActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            HomeworkManagerAppTheme {
                HomeworkManagerApp()
            }
        }
    }
}