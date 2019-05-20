$(() => {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/TaskHub").build();
    connection.start();

    $("#add").on('click', function () { 
        const task = $("#task").val();
        connection.invoke("addtask", task);
        $("#task").val('');
       
    });
    connection.on("NewTask", newtask => {
        console.log(newtask)
        $("#task-table").append(`<tr id="${newtask.id}-task-row"><td>${newtask.task}</td> 
          <td> <button class="btn btn-success assign" id="${newtask.id}-assign" data-id="${newtask.id} ">Assign to self</button></td></tr>` )        
    });

///
    $('#task-table').on('click', '.assign', function () {
        const chore = {
            id: $(this).data('id'),
            userNameAssigned: $('#userEmail').val()  
        };
        connection.invoke('AcceptTask', chore )
    });

    connection.on('AcceptedTask', chore => {
        $(`#${chore.id}-task-row`).find('td:eq(1)').html(`<td><button class="btn btn-danger" disabled> ${chore.userAssigned} is doing task</button></td>`);

    });
    connection.on('IAcceptedTask', chore => {
     console.log('accepted chore');
    $(`#${chore.id}-task-row`).find('td:eq(1)').html(`<td><button class="btn btn-info done" data-id= ${chore.id}>Done!</button></td>`);

    });

///
    
   

    $('#task-table').on('click', '.done', function () {
        const taskId = $(this).data('id');
        connection.invoke("TaskCompleted", taskId);
    });

    connection.on('TaskCompleted', taskid => {
        $(`#${taskid}-task-row`).remove();

    });


 ///
   $('task-table').on('click', '.assign', function () {
    const taskId = $(this).data('id');
    connection.invoke("TaskAssigned", taskId);
   });

connection.on("Assigned", updateTask => {
    console.log(updateTask)
    $(`#${updateTask.id}-task-row`).append(`<td>${updateTask.userAssigned} is doing this task</td>`),
        $(`${updateTask.id}-assign`).attr("disabled", true);
});

connection.on("IAssigned", updateTask => {
    console.log(updateTask)
    $(`#${updateTask.id}-task-row`).append(`<td><button class="btn btn-danger done" data-id="${updateTask.id}"> Done!</button></td>`),
        $(`${updateTask.id}-assign`).attr("disabled", true);
});////
});
