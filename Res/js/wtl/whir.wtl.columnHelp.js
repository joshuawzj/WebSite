var whir = window.whir || { wtl: {} };
whir.wtl = whir.wtl || {};
whir.wtl.columnHelp = {
    columnArray: [],
    //栏目
    Column: function () {
        this.id = 0;
        this.name = "";
        this.field = new Array();
    },
    //是否绑定栏目
    getIsBindColumn: function () {
        return $("#cbNotBindColumn").is(":checked") == false;
    },
    //获取栏目
    getColumn: function (id) {
        if (whir.wtl.columnHelp.columnArray != null) {
            for (var i in whir.wtl.columnHelp.columnArray) {
                var column = whir.wtl.columnHelp.columnArray[i];
                if (column != undefined && column.id != undefined && column.id == id) {
                    return column;
                }
            }
        }
        return null;
    },
    //获取栏目是否存在
    getColumnExists: function (id) {
        return whir.wtl.columnHelp.getColumn(id) != null;
    }
}