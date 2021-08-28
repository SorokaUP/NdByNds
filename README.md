Программа предназначена для формирования файлов XML из Excel для подачи сведений в отчет по "Налоговой декларации по НДС" в систему СБИС.
Программа включает интерфейсную часть, состоящую из одной формы с настройками для начала формирования декларации и обработчик. 

Для использования программы потребуется Excel файл формата .xls или .xlsx оформленный по шаблону Налоговой декларации: книга покупок, книга продаж, журнал выставленных счетов-фактур и журнал полученных счетов-фактур.
На форме производится выбор режимов работы: 
* Формирование файла XML из Excel
* Проверка XML по схеме
* Подсчет сумм итогового XML файла

Выбор типа книги:
* Книга покупок
* Книга продаж
* Журнал выставленных счетов-фактур
* Журнал полученных счетов-фактур

Далее указывается версия СБИС (актуальная на момент последнего коммита: 5.08).
Номер корректировки от 0 до 99.
После чего формируется файл формата XML в соответствии с выбранным типом книги.
