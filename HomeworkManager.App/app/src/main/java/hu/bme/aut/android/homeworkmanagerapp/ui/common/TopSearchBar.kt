package hu.bme.aut.android.homeworkmanagerapp.ui.common

import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Search
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.tooling.preview.Preview

@Composable
fun TopSearchBar(
    modifier: Modifier = Modifier,
    onSearch: () -> Unit,
    enabled: Boolean = true,
) {
    NormalTextField(
        modifier = modifier.fillMaxWidth(),
        value = "",
        label = "Search",
        onValueChange = { },
        leadingIcon = { },
        trailingIcon = {
            IconButton(onClick = onSearch) {
                Icon(Icons.Default.Search, null)
            }
        },
        onDone = { }
    )
}

@Preview(showBackground = true)
@Composable
fun SearchBar_Preview() {
    Box(modifier = Modifier.fillMaxSize()) {
        TopSearchBar(
            onSearch = {},
            modifier = Modifier.align(Alignment.TopCenter),
        )
    }
}