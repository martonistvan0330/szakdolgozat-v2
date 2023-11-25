package hu.bme.aut.android.homeworkmanagerapp.domain.model

data class Pageable<T>(
    val items: List<T>,
    val totalCount: Int
)